using System;
using System.Drawing;
using System.Drawing.Imaging;

namespace PaintESPE.Raster
{
    public unsafe class FastBitmap : IDisposable
    {
        public byte* BasePointer => _basePointer;
        public int Stride => _stride;
        private Bitmap _bitmap;
        private BitmapData _bitmapData;
        private byte* _basePointer;
        private int _width;
        private int _height;
        private int _stride;
        private bool _isLocked = false;

        public int Width => _width;
        public int Height => _height;
        public Bitmap Bitmap => _bitmap;

        public FastBitmap(Bitmap bitmap)
        {
            _bitmap = bitmap;
            _width = bitmap.Width;
            _height = bitmap.Height;
        }

        public void Bloquear()
        {
            if (_isLocked) return;
            
            // Format32bppArgb is standard (4 bytes per pixel: BGRA)
            _bitmapData = _bitmap.LockBits(
                new Rectangle(0, 0, _width, _height),
                ImageLockMode.ReadWrite,
                PixelFormat.Format32bppArgb);
            
            _basePointer = (byte*)_bitmapData.Scan0.ToPointer();
            _stride = _bitmapData.Stride;
            _isLocked = true;
        }

        public void Desbloquear()
        {
            if (_isLocked && _bitmapData != null)
            {
                _bitmap.UnlockBits(_bitmapData);
                _bitmapData = null;
                _basePointer = null;
                _isLocked = false;
            }
        }

        public void SetPixelRapido(int x, int y, Color color)
        {
            if (x < 0 || x >= _width || y < 0 || y >= _height) return;

            byte* pixel = _basePointer + (y * _stride) + (x * 4);
            pixel[0] = color.B;
            pixel[1] = color.G;
            pixel[2] = color.R;
            pixel[3] = color.A;
        }

        public Color GetPixelRapido(int x, int y)
        {
            if (x < 0 || x >= _width || y < 0 || y >= _height) return Color.Transparent;

            byte* pixel = _basePointer + (y * _stride) + (x * 4);
            return Color.FromArgb(pixel[3], pixel[2], pixel[1], pixel[0]);
        }

        public void Dispose()
        {
            Desbloquear();
        }
    }
}

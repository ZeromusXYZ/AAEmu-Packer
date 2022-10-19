using System;
using System.IO;

namespace AAPacker;
// Sources:
// https://stackoverflow.com/questions/6949441/how-to-expose-a-sub-section-of-my-stream-to-a-user
// https://social.msdn.microsoft.com/Forums/vstudio/en-US/c409b63b-37df-40ca-9322-458ffe06ea48/how-to-access-part-of-a-filestream-or-memorystream?forum=netfxbcl

internal class PackerSubStream : Stream
{
    private readonly long _baseOffset;
    private readonly Stream _baseStream;
    private readonly long _length;
    private long _position;

    public PackerSubStream(Stream baseStream, long offset, long length)
    {
        if (baseStream == null) throw new ArgumentNullException("baseStream");
        if (!baseStream.CanRead) throw new ArgumentException("can't read base stream");
        if (offset < 0) throw new ArgumentOutOfRangeException("offset");

        this._baseStream = baseStream;
        _baseOffset = offset;
        this._length = length;

        if (baseStream.CanSeek)
        {
            baseStream.Seek(offset, SeekOrigin.Begin);
        }
        else
        {
            // read it manually...
            const int bufferSize = 512;
            var buffer = new byte[bufferSize];
            while (offset > 0)
            {
                var read = baseStream.Read(buffer, 0, offset < bufferSize ? (int)offset : bufferSize);
                offset -= read;
            }
        }
    }

    public override long Length
    {
        get
        {
            CheckDisposed();
            return _length;
        }
    }

    public override bool CanRead
    {
        get
        {
            CheckDisposed();
            return true;
        }
    }

    public override bool CanWrite
    {
        get
        {
            CheckDisposed();
            return false;
        }
    }

    public override bool CanSeek
    {
        get
        {
            CheckDisposed();
            return false;
        }
    }

    public override long Position
    {
        get
        {
            CheckDisposed();
            return _position;
        }
        set
        {
            _position = _position > _length ? _length : value;
            _baseStream.Position = _baseOffset + _position;
        }
    }

    public override int Read(byte[] buffer, int offset, int count)
    {
        CheckDisposed();
        var remaining = _length - _position;
        if (remaining <= 0) return 0;
        if (remaining < count) count = (int)remaining;
        var read = _baseStream.Read(buffer, offset, count);
        _position += read;
        return read;
    }

    private void CheckDisposed()
    {
        if (_baseStream == null) throw new ObjectDisposedException(GetType().Name);
    }

    public override long Seek(long offset, SeekOrigin origin)
    {
        throw new NotSupportedException();
    }

    public override void SetLength(long value)
    {
        throw new NotSupportedException();
    }

    public override void Flush()
    {
        CheckDisposed();
        _baseStream.Flush();
    }

    protected override void Dispose(bool disposing)
    {
        base.Dispose(disposing);
        /*
        if (disposing)
        {
            if (baseStream != null)
            {
                try { baseStream.Dispose(); }
                catch { }
                baseStream = null;
            }
        }
        */
    }

    public override void Write(byte[] buffer, int offset, int count)
    {
        throw new NotImplementedException();
    }
}
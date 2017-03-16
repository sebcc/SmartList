using System;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using PCLStorage;

namespace SmartList.Test
{
    public class FileStub : IFile
    {
        readonly string content;

        public FileStub (string content)
        {
            this.content = content;
        }

        public string WrittenContent
        {
            get
            {
                var sr = new StreamReader (File.OpenRead ("test"));
                return sr.ReadToEnd ();
            }
        }

        public string Name
        {
            get
            {
                throw new NotImplementedException ();
            }
        }

        public string Path
        {
            get
            {
                throw new NotImplementedException ();
            }
        }

        public Task WriteAllTextAsync (string contents)
        {
            return Task.FromResult<object> (null);
        }

        public Task<string> ReadAllTextAsync ()
        {
            return Task.FromResult (this.content);
        }

        public Task DeleteAsync (CancellationToken cancellationToken = default (CancellationToken))
        {
            throw new NotImplementedException ();
        }

        public Task MoveAsync (string newPath, NameCollisionOption collisionOption = NameCollisionOption.ReplaceExisting, CancellationToken cancellationToken = default (CancellationToken))
        {
            throw new NotImplementedException ();
        }

        public Task<Stream> OpenAsync (PCLStorage.FileAccess fileAccess, CancellationToken cancellationToken = default (CancellationToken))
        {
            if (fileAccess == PCLStorage.FileAccess.Read)
            {
                MemoryStream stream = new MemoryStream ();
                StreamWriter writer = new StreamWriter (stream);
                writer.Write (this.content);
                writer.Flush ();
                stream.Position = 0;
                return Task.FromResult (stream as Stream);
            }
            else
            {
                return Task.FromResult (File.OpenWrite ("test") as Stream);

            }
        }

        public Task RenameAsync (string newName, NameCollisionOption collisionOption = NameCollisionOption.FailIfExists, CancellationToken cancellationToken = default (CancellationToken))
        {
            throw new NotImplementedException ();
        }
    }
}

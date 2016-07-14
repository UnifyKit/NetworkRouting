using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace NetworkRouting
{
    internal class QuadTreeIndexFile<T>
    {
        private QuadtreeIndex qtreeIndex;
        private string qtreeFilePath;
        private FileAccess fileAccess;

        private IFile fileInstance;
        private Stream fileStream;
        private bool isOpen;

        private QuadTreeIndexFile()
        { }

        public QuadTreeIndexFile(string qtreeFilePath, QuadtreeIndex quadTreeIndex)
            : this(qtreeFilePath, quadTreeIndex, FileAccess.Read)
        { }

        public QuadTreeIndexFile(string qtreeFilePath, QuadtreeIndex quadTreeIndex, FileAccess fileAccess)
        {
            this.qtreeFilePath = qtreeFilePath;
            this.qtreeIndex = quadTreeIndex;
            this.fileAccess = fileAccess;

            this.fileInstance = FileResolver.Instance;
            this.fileInstance.Path = qtreeFilePath;
        }

        public async void Open()
        {
            if (!isOpen || fileStream == null)
            {
                fileStream = await fileInstance.OpenAsync(fileAccess);
                isOpen = true;
            }
        }

        public void Close()
        {
            if (isOpen || fileStream != null)
            {
                fileStream.Dispose();
                isOpen = false;
            }
        }

        public void Save()
        {

        }


        public void Read()
        { }
    }
}

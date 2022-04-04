using System;
using System.Collections.Generic;
using System.Text;

namespace MinoTool
{
    public class Assets
    {
        private readonly FilesRepository _filesRepository;

        public Assets()
        {
            _filesRepository = new FilesRepository();
        }

        /// <summary>From root: Assets/</summary>
        public T LoadAsset<T>(string path) where T: Asset
        {
            if (typeof(T) == typeof(TextAsset))
            {
                return new TextAsset(_filesRepository.GetTextFile(path)) as T;
            }

            return default;
        }
    }
}

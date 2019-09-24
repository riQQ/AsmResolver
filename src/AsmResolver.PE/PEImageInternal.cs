// AsmResolver - Executable file format inspection library 
// Copyright (C) 2016-2019 Washi
// 
// This library is free software; you can redistribute it and/or
// modify it under the terms of the GNU Lesser General Public
// License as published by the Free Software Foundation; either
// version 3.0 of the License, or (at your option) any later version.
// 
// This library is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU
// Lesser General Public License for more details.
// 
// You should have received a copy of the GNU Lesser General Public
// License along with this library; if not, write to the Free Software
// Foundation, Inc., 51 Franklin Street, Fifth Floor, Boston, MA  02110-1301  USA

using System;
using System.Collections.Generic;
using AsmResolver.PE.File;
using AsmResolver.PE.File.Headers;
using AsmResolver.PE.Imports;
using AsmResolver.PE.Imports.Internal;
namespace AsmResolver.PE
{
    internal class PEImageInternal : PEImageBase
    {
        private readonly PEFile _peFile;

        public PEImageInternal(PEFile peFile)
        {
            _peFile = peFile ?? throw new ArgumentNullException(nameof(peFile));
        }

        protected override IList<ModuleImportEntryBase> GetImports()
        {
            var importDirectory = _peFile.OptionalHeader.DataDirectories[OptionalHeader.ImportDirectoryIndex];
            var directoryReader = _peFile.CreateDataDirectoryReader(importDirectory);
            return new ModuleImportEntryList(_peFile, directoryReader);
        }
        
    }
}
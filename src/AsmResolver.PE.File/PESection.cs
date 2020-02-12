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
using AsmResolver.PE.File.Headers;

namespace AsmResolver.PE.File
{

    public static class PESectionHelper
    {
        public static List<PESection> Copy(this IList<PESection> collection)
        {
            List<PESection> value = new List<PESection>();
            foreach (var item in collection)
            {
                value.Add(new PESection(item));
            }

            return value;
        }
    }

    /// <summary>
    /// Represents a single section in a portable executable (PE) file.
    /// </summary>
    public class PESection : IReadableSegment
    {
        /// <summary>
        /// Creates a new empty section.
        /// </summary>
        /// <param name="name">The name of the section.</param>
        /// <param name="characteristics">The section flags.</param>
        public PESection(string name, SectionFlags characteristics)
        {
            Header = new SectionHeader(name, characteristics);
        }

        /// <summary>
        /// Copy a new section.
        /// </summary>
        /// <param name="section">The section to be copied.</param>
        public PESection(PESection section) 
            : this (section.Header, section.Contents)
        {
        }

        /// <summary>
        /// Creates a new section with the provided contents.
        /// </summary>
        /// <param name="header">The header to associate to the section.</param>
        /// <param name="contents">The contents of the section.</param>
        public PESection(SectionHeader header, IReadableSegment contents)
        {
            Header = header;
            Contents = contents;
        }

        /// <summary>
        /// Copy a section with the provided contents.
        /// </summary>
        /// <param name="header">The section's header to be copied.</param>
        /// <param name="contents">The contents of the section.</param>
        public PESection(SectionHeader header, ISegment contents)
        {
            Header = new SectionHeader(header);
            Contents = contents;

        }

        /// <summary>
        /// Gets or sets the header associated to the section.
        /// </summary>
        public SectionHeader Header
        {
            get;
        }

        /// <summary>
        /// Gets or sets the contents of the section.
        /// </summary>
        public ISegment Contents
        {
            get;
            set;
        }

        /// <summary>
        /// Gets a value indicating whether the section is readable using a binary stream reader.
        /// </summary>
        public bool IsReadable => Contents is IReadableSegment;

        /// <inheritdoc />
        public uint FileOffset => Contents?.FileOffset ?? Header.PointerToRawData;

        /// <inheritdoc />
        public uint Rva => Contents?.Rva ?? Header.VirtualAddress;

        /// <inheritdoc />
        public bool CanUpdateOffsets => true;

        /// <inheritdoc />
        public void UpdateOffsets(uint newFileOffset, uint newRva)
        {
            Contents.UpdateOffsets(newFileOffset, newRva);
        }

        /// <inheritdoc />
        public uint GetPhysicalSize()
        {
            return Contents.GetPhysicalSize();
        }

        /// <inheritdoc />
        public uint GetVirtualSize()
        {
            return Contents.GetVirtualSize();
        }

        /// <inheritdoc />
        public IBinaryStreamReader CreateReader(uint fileOffset, uint size)
        {
            if (!IsReadable)
                throw new InvalidOperationException("Section contents is not readable.");
            return ((IReadableSegment) Contents).CreateReader(fileOffset, size);
        }

        /// <inheritdoc />
        public void Write(IBinaryStreamWriter writer)
        {
            Contents.Write(writer);
        }

        /// <inheritdoc />
        public override string ToString()
        {
            return Header.Name;
        }
    }
}
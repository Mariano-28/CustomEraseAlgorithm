// Custom Erase - Secure File Deletion Algorithm
// Copyright (C) 2007–2025 Mariano Ortu <https://www.sicurpas.it/>
//
// This Algorithm is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
//
// This Algorithm is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with this class. If not, see <https://www.gnu.org/licenses/gpl-3.0.html>.

using System.Collections.Generic;

namespace CustomEraserLib
{
    public static class PassSuggestionEngine
    {
        public static List<string> GetSuggestedAlgorithm()
        {
            return new List<string>
            {
                "Random",
                "Zero(0x00)",
                "One(0xFF)",
                "Random",
                "Random"
            };
        }
    }
}
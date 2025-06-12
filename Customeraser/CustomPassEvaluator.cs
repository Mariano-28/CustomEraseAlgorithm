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

using System;
using System.Collections.Generic;

namespace CustomEraserLib
{
    public class CustomPassEvaluator
    {
        // Stores the list of input string representations of passes
        private readonly List<string> _items;

        // Constructor that initializes the evaluator with a list of input items
        public CustomPassEvaluator(IEnumerable<string> items)
        {
            _items = new List<string>(items);
        }

        // Evaluates the overall strength of the given sequence of passes
        // Returns a tuple: numeric score, textual label, and a value for visual display (0–100)
        public (int Score, string StrengthLabel, int VisualValue) EvaluateStrength()
        {
            var passes = new List<CustomPassType>();

            // Convert each string to its corresponding pass type (if possible)
            foreach (string text in _items)
            {
                if (text.StartsWith("Zero", StringComparison.OrdinalIgnoreCase))
                {
                    passes.Add(CustomPassType.Zeros);
                }
                else if (text.StartsWith("One", StringComparison.OrdinalIgnoreCase))
                {
                    passes.Add(CustomPassType.Ones);
                }
                else if (text.Equals("Random", StringComparison.OrdinalIgnoreCase))
                {
                    passes.Add(CustomPassType.Random);
                }
                else
                {
                    // Attempt to parse using enum if not matched by name
                    if (Enum.TryParse(text, true, out CustomPassType parsed))
                    {
                        passes.Add(parsed);
                    }
                }
            }

            // Compute numeric score based on the combination of passes
            int score = CalculateScore(passes);

            // Assign strength label and visual value based on score thresholds
            string label;
            int visualValue;

            if (score <= 4)
            {
                label = "Weak";
                visualValue = 25;
            }
            else if (score <= 8)
            {
                label = "Moderate";
                visualValue = 50;
            }
            else if (score <= 14)
            {
                label = "Strong";
                visualValue = 75;
            }
            else
            {
                label = "Very Strong";
                visualValue = 100;
            }

            return (score, label, visualValue);
        }

        // Calculates a numeric score based on the types and diversity of passes
        private int CalculateScore(List<CustomPassType> passes)
        {
            if (passes.Count == 0)
                return 0;

            int score = 0;
            int weakCount = 0;
            int mediumCount = 0;
            int strongCount = 0;

            // Keeps track of distinct pass types
            var distinctPasses = new HashSet<CustomPassType>();

            foreach (CustomPassType pass in passes)
            {
                distinctPasses.Add(pass);

                if (pass == CustomPassType.Random)
                {
                    score += 3;
                    strongCount++;
                }
                else if (pass == CustomPassType.Zeros || pass == CustomPassType.Ones)
                {
                    score += 1;
                    weakCount++;
                }
                else
                {
                    score += 2;
                    mediumCount++;
                }
            }

            int total = passes.Count;

            // Penalize if all passes are of the same type
            if (distinctPasses.Count == 1)
                return Math.Min(score, 4);

            // Reward diversity
            score += Math.Min(distinctPasses.Count, 3);

            // Reward high security if strong passes and diversity present
            if (strongCount >= 2 && distinctPasses.Count >= 3)
                return Math.Min(score + 2, 20);

            if (strongCount >= 1)
                return Math.Min(score, 20);

            // Penalize if majority are weak passes
            if ((double)weakCount / total > 0.6)
                return Math.Min(score, 8);

            return score;
        }
    }
}
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
using System.IO;
using System.Threading;
using System.Security.Cryptography;


namespace CustomEraserLib
{
    public static class CustomEraser
    {
        /// <summary>
        /// Performs a secure erase of the specified file by overwriting it with a custom sequence of passes and then deleting it.
        /// </summary>
        /// <param name="filePath">Full path of the file to securely erase. Must point to an existing and accessible file.</param>
        /// <param name="passSequence">
        /// Sequence of overwrite passes to apply. 
        /// Supported values: 
        /// - "Zero" (0x00),
        /// - "One" (0xFF),
        /// - "Random" (cryptographically secure random bytes).
        /// The sequence is applied in the exact order provided.
        /// </param>
        /// <param name="cancellationToken">
        /// Optional token to support cooperative cancellation.
        /// If cancellation is requested, the operation will terminate gracefully as soon as possible, 
        /// without compromising data integrity up to that point.
        /// </param>
        /// <returns>
        /// Returns <c>true</c> if the operation completes successfully; 
        /// <c>false</c> if the process fails or is interrupted by cancellation.
        /// </returns>
        /// <remarks>
        /// This method executes a secure, multi-pass file erase process intended to mitigate forensic recovery.
        ///
        /// The algorithm performs the following steps:
        /// 1. Validates input and file accessibility.
        /// 2. Clears restrictive attributes (read-only, hidden, system).
        /// 3. Renames the file to a random name to reduce metadata traceability.
        /// 4. Opens the file with exclusive access to avoid conflicts.
        /// 5. For each pass in the <paramref name="passSequence"/>, 
        ///    overwrites the file content in 8KB buffered blocks.
        /// 6. Ensures each pass is flushed to disk to guarantee persistence.
        /// 7. After all passes, truncates the file to zero bytes.
        /// 8. Deletes the file permanently, bypassing the recycle bin.
        ///
        /// This approach complies with basic secure deletion standards and allows flexible user-defined patterns.
        ///
        /// Note: While hardware differences, file system behaviors, and journaling mechanisms can affect data recovery chances,
        /// the official documentation reports that residual recoverability remains around 5% under typical conditions,
        /// making this algorithm highly effective for secure data deletion.
        ///
        /// Cancellation support is optional and not demonstrated in the sample project.
        /// </remarks>

        public static bool SecureErase(string filePath, CustomPassType[] passSequence, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(filePath) || !File.Exists(filePath))
            {
                ShowError("File not found:\n" + filePath);
                return false;
            }

            try
            {
                var fileInfo = new FileInfo(filePath);

                // Remove protected attibutes to allow overwriting
                fileInfo.Attributes &= ~(FileAttributes.ReadOnly | FileAttributes.Hidden | FileAttributes.System);

                // Rename the file with a random name to reduce traces of original filename
                string directory = fileInfo.DirectoryName ?? throw new IOException("Unable to determine directory.");
                string randomFileName = Path.Combine(directory, Path.GetRandomFileName());

                while (File.Exists(randomFileName))
                {
                    randomFileName = Path.Combine(directory, Path.GetRandomFileName());
                }

                File.Move(filePath, randomFileName);

                // Update path to new file name
                filePath = randomFileName;
                fileInfo = new FileInfo(filePath);

                long length = fileInfo.Length;

                using (var fs = new FileStream(filePath, FileMode.Open, FileAccess.Write, FileShare.None))
                using (var rng = RandomNumberGenerator.Create())
                {
                    byte[] buffer = new byte[8192]; // 8 KB buffer for optimal performance and memory usage

                    foreach (var pass in passSequence)
                    {
                        fs.Seek(0, SeekOrigin.Begin);
                        long totalWritten = 0;

                        while (totalWritten < length)
                        {
                            cancellationToken.ThrowIfCancellationRequested();

                            int toWrite = (int)Math.Min(buffer.Length, length - totalWritten);

                            switch (pass)
                            {
                                case CustomPassType.Zeros:
                                    Array.Clear(buffer, 0, toWrite);
                                    break;

                                case CustomPassType.Ones:
                                    FillBuffer(buffer, 0xFF, toWrite);
                                    break;

                                case CustomPassType.Random:
                                    rng.GetBytes(buffer, 0, toWrite);
                                    break;
                            }

                            fs.Write(buffer, 0, toWrite);
                            totalWritten += toWrite;
                        }

                        // Flush physical write to disk (not just cache)
                        fs.Flush(true);
                    }

                    // Truncate file to zero length to remove residual data
                    fs.SetLength(0);
                }

                // Permanently delete the file
                File.Delete(filePath);

                return true;
            }
            catch (UnauthorizedAccessException)
            {
                ShowError("Access denied. The file may be in use or protected.");
                return false;
            }
            catch (IOException ex) when ((ex.HResult & 0xFFFF) == 32) // ERROR_SHARING_VIOLATION
            {
                ShowError("File is currently in use by another process.");
                return false;
            }
            catch (OperationCanceledException)
            {
                ShowError("Operation canceled by the user.");
                return false;
            }
            catch (Exception ex)
            {
                ShowError("Error during secure deletion:\n" + ex.Message);
                return false;
            }
        }

        /// <summary>
        /// Fills the buffer with the specified byte value.
        /// </summary>
        /// <param name="buffer">Byte array to fill.</param>
        /// <param name="value">Byte value to set.</param>
        /// <param name="count">Number of bytes to fill.</param>
        private static void FillBuffer(byte[] buffer, byte value, int count)
        {
            for (int i = 0; i < count; i++)
                buffer[i] = value;
        }

        /// <summary>
        /// Shows an error message box to the user. In this case Console
        /// </summary>
        /// <param name="message">Message text to display.</param>
        private static void ShowError(string message)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("ERROR: " + message);
            Console.ResetColor();
        }
    }

    /// <summary>
    /// Supported overwrite pass types.
    /// </summary>
    public enum CustomPassType
    {
        /// <summary>
        /// Overwrites with zeros (0x00).
        /// </summary>
        Zeros,

        /// <summary>
        /// Overwrites with ones (0xFF).
        /// </summary>
        Ones,

        /// <summary>
        /// Overwrites with cryptographically secure random bytes.
        /// </summary>
        Random
    }
}
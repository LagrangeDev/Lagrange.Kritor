using System.Collections;
using System.Collections.Generic;
using System.Text;
using QRCoder;

namespace Lagrange.Kritor.Utility;

public class QRCodeUtility {
    // "▄", "▀", " ", "█"
    public static string BuildConsoleString(string text) {
        StringBuilder console = new();

        QRCodeData qrcode = QRCodeGenerator.GenerateQrCode(text, QRCodeGenerator.ECCLevel.L);

        List<BitArray> matrix = qrcode.ModuleMatrix;

        int length = 17 + (qrcode.Version * 4);

        int padding = (matrix.Count - length) / 2;

        int yEnd = padding + length;
        int xEnd = padding + length;

        console.Append('\n');
        for (int y = padding; y < yEnd; y += 2) {
            for (int x = padding; x < xEnd; x++) {
                bool up = matrix[y][x];
                bool down = (y + 1) < matrix.Count && matrix[y + 1][x];

                if (up && down) console.Append('█');
                else if (up && !down) console.Append('▀');
                else if (!up && down) console.Append('▄');
                else if (!up && !down) console.Append(' ');
            }
            console.Append('\n');
        }

        return console.ToString();
    }
}
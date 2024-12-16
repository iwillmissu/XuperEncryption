using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO.Compression;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace XuperEncryption
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }


        private void btnEncode_Click(object sender, EventArgs e)
        {
            try
            {
                // 获取输入文本
                var input = txtInput.Text;

                if (string.IsNullOrEmpty(input))
                {
                    return;
                }

                // 将字符串转换为字节数组
                var originalBytes = Encoding.UTF8.GetBytes(input);

                // 使用 GZip 压缩
                var compressedBytes = Compress(originalBytes);

                // 使用 Base64 编码
                var encoded = Convert.ToBase64String(compressedBytes);

                // 显示编码后的结果
                txtOutput.Text = encoded;
            }
            catch (Exception ex)
            {
                MessageBox.Show("编码失败: " + ex.Message, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnDecode_Click(object sender, EventArgs e)
        {
            try
            {
                // 获取输入的 Base64 字符串
                var base64String = txtOutput.Text;

                if (string.IsNullOrEmpty(base64String))
                {
                    return;
                }

                // 将 Base64 字符串解码为字节数组
                var compressedBytes = Convert.FromBase64String(base64String);

                // 使用 GZip 解压缩
                var decompressedBytes = Decompress(compressedBytes);

                // 将字节数组转换回字符串
                var decoded = Encoding.UTF8.GetString(decompressedBytes);

                // 显示解码后的结果
                txtInput.Text = decoded;
            }
            catch (Exception ex)
            {
                MessageBox.Show("解码失败: " + ex.Message, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private static byte[] Compress(byte[] data)
        {
            using (var compressedStream = new MemoryStream())
            {
                using (var gzipStream = new GZipStream(compressedStream, CompressionMode.Compress))
                {
                    gzipStream.Write(data, 0, data.Length);
                    gzipStream.Close();
                    return compressedStream.ToArray();
                }
            }
        }

        private static byte[] Decompress(byte[] compressedData)
        {
            using (var compressedStream = new MemoryStream(compressedData))
            {
                using (var resultStream = new MemoryStream())
                {
                    using (var gzipStream = new GZipStream(compressedStream, CompressionMode.Decompress))
                    {
                        byte[] buffer = new byte[4096];
                        int bytesRead;
                        while ((bytesRead = gzipStream.Read(buffer, 0, buffer.Length)) > 0)
                        {
                            resultStream.Write(buffer, 0, bytesRead);
                        }
                        return resultStream.ToArray();
                    }
                }
            }
        }
    }
}

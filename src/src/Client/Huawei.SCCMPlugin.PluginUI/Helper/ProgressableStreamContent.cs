using Huawei.SCCMPlugin.Core.Exceptions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Huawei.SCCMPlugin.FusionDirector.PluginUI.Helper
{
    public class ProgressableStreamContent : HttpContent
    {
        static event EventHandler<AggregateExceptionArgs> AggregateExceptionCatched;
        /// <summary>
        /// Lets keep buffer of 20kb
        /// </summary>
        private const int defaultBufferSize = 5 * 4096;

        private HttpContent content;
        private int bufferSize;
        //private bool contentConsumed;
        private Action<long, long> progress;

        public ProgressableStreamContent(HttpContent content, Action<long, long> progress) : this(content, defaultBufferSize, progress) { }

        public ProgressableStreamContent(HttpContent content, int bufferSize, Action<long, long> progress)
        {
            if (content == null)
            {
                throw new ArgumentNullException("content");
            }
            if (bufferSize <= 0)
            {
                throw new ArgumentOutOfRangeException("bufferSize");
            }

            this.content = content;
            this.bufferSize = bufferSize;
            this.progress = progress;

            foreach (var h in content.Headers)
            {
                this.Headers.Add(h.Key, h.Value);
            }
        }

        protected override Task SerializeToStreamAsync(Stream stream, TransportContext context)
        {
            AggregateExceptionCatched += new EventHandler<AggregateExceptionArgs>(Program_AggregateExceptionCatched);
            return Task.Factory.StartNew(() =>
            {
                var buffer = new Byte[this.bufferSize];
                long size;
                TryComputeLength(out size);
                var uploaded = 0;
                using (var sinput = content.ReadAsStreamAsync().Result)
                {
                    while (true)
                    {
                        try
                        {
                            var length = sinput.Read(buffer, 0, buffer.Length);
                            if (length <= 0) break;

                            //downloader.Uploaded = uploaded += length;
                            uploaded += length;
                            progress?.Invoke(uploaded, size);

                            //System.Diagnostics.Debug.WriteLine($"Bytes sent {uploaded} of {size}");

                            stream.Write(buffer, 0, length);
                            stream.Flush();
                        }
                        catch (Exception ex)
                        {
                            AggregateExceptionArgs args = new AggregateExceptionArgs()
                            {
                                AggregateException = new AggregateException(ex)
                            };
                            //使用主线程委托代理，处理子线程 异常
                            //这种方式没有阻塞 主线程或其他线程
                            AggregateExceptionCatched?.Invoke(null, args);
                            break;
                        }

                    }
                }
                stream.Flush();
            });
        }

        protected override bool TryComputeLength(out long length)
        {
            length = content.Headers.ContentLength.GetValueOrDefault();
            return true;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                content.Dispose();
            }
            base.Dispose(disposing);
        }
        void Program_AggregateExceptionCatched(object sender, AggregateExceptionArgs e)
        {
            foreach (var item in e.AggregateException.InnerExceptions)
            {
                LogUtil.HWLogger.UI.ErrorFormat("异常类型：{0}{1}来自：{2}{3}异常内容：{4}",
                    item.GetType(), Environment.NewLine, item.Source,
                    Environment.NewLine, item.Message);
            }
        }

    }
}

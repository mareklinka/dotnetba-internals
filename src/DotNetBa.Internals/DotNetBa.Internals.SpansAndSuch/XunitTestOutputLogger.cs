using System.Text;
using BenchmarkDotNet.Loggers;
using Xunit.Abstractions;

namespace DotNetBa.Internals.SpansAndSuch
{
    /// <summary>
    /// A class that writes output to the test output for Xunit. This class cannot be inherited.
    /// </summary>
    public sealed class XunitTestOutputLogger : ILogger
    {
        private readonly StringBuilder _sb = new StringBuilder();

        /// <summary>
        /// The <see cref="ITestOutputHelper"/> in use. This field is read-only.
        /// </summary>
        private readonly ITestOutputHelper _helper;

        /// <summary>
        /// Initializes a new instance of the <see cref="XunitTestOutputLogger"/> class.
        /// </summary>
        /// <param name="helper">The <see cref="ITestOutputHelper"/> to use.</param>
        public XunitTestOutputLogger(ITestOutputHelper helper)
        {
            _helper = helper;
        }

        public void Write(LogKind logKind, string text)
        {
            _sb.Append(text);
        }

        public void WriteLine()
        {
            _helper.WriteLine(_sb.ToString());
            _sb.Clear();
        }

        public void WriteLine(LogKind logKind, string text)
        {
            _sb.Append(text);
            _helper.WriteLine(_sb.ToString());
            _sb.Clear();
        }

        public void Flush()
        {
            _helper.WriteLine(_sb.ToString());
            _sb.Clear();
        }
    }
}
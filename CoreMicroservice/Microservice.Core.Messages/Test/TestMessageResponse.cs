using System;
using System.Collections.Generic;
using System.Text;

namespace Microservice.Core.Messages.Test
{
    public class TestMessageResponse
    {
        public Guid Id { get; set; }

        public string Text { get; set; }

        public int Value { get; set; }
    }
}

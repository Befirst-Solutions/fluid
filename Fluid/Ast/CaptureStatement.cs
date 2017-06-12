﻿using System.Collections.Generic;
using System.IO;
using System.Text.Encodings.Web;

namespace Fluid.Ast
{
    public class CaptureStatement : TagStatement
    {
        public CaptureStatement(string identifier, IList<Statement> statements): base(statements)
        {
            Identifier = identifier;
        }

        public string Identifier { get; }

        public override Completion WriteTo(TextWriter writer, TextEncoder encoder, TemplateContext context)
        {
            var completion = Completion.Normal;

            using (var sw = new StringWriter())
            {
                for (var index = 0; index < Statements.Count; index++)
                {
                    completion = Statements[index].WriteTo(sw, encoder, context);

                    if (completion != Completion.Normal)
                    {
                        // Stop processing the block statements
                        // We return the completion to flow it to the outer loop
                        break;
                    }
                }

                context.SetValue(Identifier, sw.ToString());
            }           

            return completion;
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheQuestionReborn.API.ContentSources
{
    public abstract class SourceBase
    {
        public ContentFactory Factory { get; set; }

        protected SourceBase()
        {
            Factory = new ContentFactory();
        }
    }
}

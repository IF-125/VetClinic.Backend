using AutoMapper;
using System;

namespace VetClinic.WebApi.Converters
{
    public class StringToTimeSpanConverter : IValueConverter<TimeSpan, string>
    {
        public string Convert(TimeSpan sourceMember, ResolutionContext context)
        {
            return sourceMember.ToString();
        }
    }
}

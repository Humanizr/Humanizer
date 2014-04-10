﻿using System;
using Xunit;

namespace Humanizer.Tests.Localisation.es
{
    public class TimeSpanHumanizeTests : AmbientCulture
    {
        public TimeSpanHumanizeTests() : base("es-ES") { }

        [Fact]
        public void TwoWeeks()
        {
            Assert.Equal("2 semanas", TimeSpan.FromDays(14).Humanize());
        }

        [Fact]
        public void OneWeek()
        {
            Assert.Equal("una semana", TimeSpan.FromDays(7).Humanize());
        }

        [Fact]
        public void SixDays()
        {
            Assert.Equal("6 días", TimeSpan.FromDays(6).Humanize());
        }

        [Fact]
        public void TwoDays()
        {
            Assert.Equal("2 días", TimeSpan.FromDays(2).Humanize());
        }

        [Fact]
        public void OneDay()
        {
            Assert.Equal("un día", TimeSpan.FromDays(1).Humanize());
        }

        [Fact]
        public void TwoHours()
        {
            Assert.Equal("2 horas", TimeSpan.FromHours(2).Humanize());
        }

        [Fact]
        public void OneHour()
        {
            Assert.Equal("una hora", TimeSpan.FromHours(1).Humanize());
        }

        [Fact]
        public void TwoMinutes()
        {
            Assert.Equal("2 minutos", TimeSpan.FromMinutes(2).Humanize());
        }

        [Fact]
        public void OneMinute()
        {
            Assert.Equal("un minuto", TimeSpan.FromMinutes(1).Humanize());
        }

        [Fact]
        public void TwoSeconds()
        {
            Assert.Equal("2 segundos", TimeSpan.FromSeconds(2).Humanize());
        }

        [Fact]
        public void OneSecond()
        {
            Assert.Equal("un segundo", TimeSpan.FromSeconds(1).Humanize());
        }

        [Fact]
        public void TwoMilliseconds()
        {
            Assert.Equal("2 milisegundos", TimeSpan.FromMilliseconds(2).Humanize());
        }

        [Fact]
        public void OneMillisecond()
        {
            Assert.Equal("un milisegundo", TimeSpan.FromMilliseconds(1).Humanize());
        }

        [Fact]
        public void NoTime()
        {
            // This one doesn't make a lot of sense but ... w/e
            Assert.Equal("0 segundos", TimeSpan.Zero.Humanize());
        }
    }
}
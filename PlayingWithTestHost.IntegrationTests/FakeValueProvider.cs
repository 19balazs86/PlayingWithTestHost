﻿namespace PlayingWithTestHost.IntegrationTests
{
  public class FakeValueProvider : IValueProvider
  {
    public const string Value = "IntegrationTest";

    public string GetValue() => Value;
  }
}

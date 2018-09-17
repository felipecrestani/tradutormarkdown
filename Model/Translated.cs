using System;
using System.Collections.Generic;

public class DetectedLanguage
{
    public string language { get; set; }
    public string score { get; set; }
}

public class Translation
{
    public string text { get; set; }
    public string to { get; set; }
}

public class Translated
{
    public DetectedLanguage detectedLanguage { get; set; }
    public List<Translation> translations { get; set; }
}
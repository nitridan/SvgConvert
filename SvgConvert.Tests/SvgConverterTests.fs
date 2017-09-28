module SvgConverterTests

open System
open Xunit
open SvgConvert
open System.IO

[<Fact>]
let ``Convert valid SVG to JS``() =
    let outputJs = File.ReadAllText(Path.Combine("TestData", "output.js"))
    let inputSvg = File.ReadAllText(Path.Combine("TestData", "minified.svg"))
    let output = SvgConverter.convertSvg inputSvg
    Assert.Equal(outputJs, output)
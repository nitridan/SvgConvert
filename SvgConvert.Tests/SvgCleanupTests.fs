module SvgCleanupTests

open System
open Xunit
open SvgConvert
open System.IO

[<Fact>]
let ``cleanupSvg called on valid svg, result trimmed`` () =
    let inputSvg = File.ReadAllText(Path.Combine("TestData", "input.svg"))
    let resultSvg = File.ReadAllText(Path.Combine("TestData", "minified.svg"))
    let output = SvgCleanup.cleanupSvg inputSvg
    Assert.Equal(resultSvg, output)

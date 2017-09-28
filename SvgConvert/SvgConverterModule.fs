namespace SvgConvert

open System
open System.Management.Automation

module SvgConverterModule =

    [<Cmdlet("Svg", "ConvertToJs")>]
    type SvgJsConverterCommandlet() =
        inherit Cmdlet()

        let mutable svgString : String = null

        [<Parameter(Mandatory=true, ValueFromPipeline = true)>]
        member this.SvgString
            with get () = svgString
            and set value = svgString <- value

        override this.ProcessRecord() = this.WriteObject(SvgConverter.convertSvg svgString)
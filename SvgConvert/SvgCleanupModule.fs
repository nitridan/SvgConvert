namespace SvgConvert

open System
open System.Management.Automation

module SvgCleanupModule =

    [<Cmdlet("Svg", "Cleanup")>]
    type SvgCleanupCommandlet() =
        inherit Cmdlet()

        let mutable svgString : String = null

        [<Parameter(Mandatory=true, ValueFromPipeline = true)>]
        member this.SvgString
            with get () = svgString
            and set value = svgString <- value

        override this.ProcessRecord() = this.WriteObject(SvgCleanup.cleanupSvg svgString)
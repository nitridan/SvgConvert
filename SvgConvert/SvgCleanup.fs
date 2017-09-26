namespace SvgConvert

open System.Xml.Linq;

module SvgCleanup =

    let xname name = XNamespace.Get(name)

    let svgNameSpace = xname "http://www.w3.org/2000/svg"
    let dcNameSpace = xname "http://purl.org/dc/elements/1.1/"
    let ccNameSpace = xname "http://creativecommons.org/ns#"
    let rdfNamespace = xname "http://www.w3.org/1999/02/22-rdf-syntax-ns#"
    let sodipodiNamespace = xname "http://sodipodi.sourceforge.net/DTD/sodipodi-0.dtd"
    let inkscapeNamespace = xname "http://www.inkscape.org/namespaces/inkscape"
    
    let redundantNamespaces = [|
        dcNameSpace
        rdfNamespace
        sodipodiNamespace
        inkscapeNamespace
        |]

    let descendants (doc: XDocument) = doc.Descendants() |> Seq.toArray

    let attributes doc = (descendants doc).Attributes() |> Seq.toArray

    let removeXElements (els: seq<XElement>) = els |> Seq.map (fun x -> x.Remove()) |> Seq.length

    let removeAttributes (attrs: seq<XAttribute>) = attrs |> Seq.map (fun x -> x.Remove()) |> Seq.length

    let cleanGarbage (doc: XDocument, redundantNamespace: XNamespace) =
        let namespaceName = redundantNamespace.NamespaceName
        printfn "Removing redundant elements for namespace: %s" namespaceName
        let elCount = 
            descendants doc
            |> Seq.filter (fun x -> x.Name.Namespace = redundantNamespace)
            |> Seq.length

        printfn "Removed %i elements" elCount

        printfn "Removing redundant attributes for namespace: %s" namespaceName
        let attribCount = 
            attributes doc
            |> Seq.filter (fun x -> 
                (x.Name.Namespace = redundantNamespace) || 
                (x.IsNamespaceDeclaration && x.Value = namespaceName))
            |> removeAttributes

        printfn "Removed %i attributes" attribCount

    let cleanSvgGarbage (doc: XDocument, redundantNamespace: XNamespace) =
        let namespaceName = redundantNamespace.NamespaceName
        printfn "Removing redundant elements for namespace: %s" namespaceName
        let redundantElements = [| "defs"; "metadata" |]
        let elCount =
            seq { 
                for el in descendants doc do
                    for redundant in redundantElements do
                        if el.Name.LocalName = redundant then yield el 
            } |> removeXElements
        printfn "Removed %i elements" elCount

        printfn "Removing redundant attributes for namespace: %s" namespaceName
        let redundantAttributes = [| "width"; "height"; "id"; "version" |]
        let attribCount =
            seq {
                for attr in attributes doc |> Seq.filter (fun x -> x.Parent.Name.Namespace = redundantNamespace) do
                    for redundant in redundantAttributes do
                        if attr.Name.LocalName = redundant then yield attr
            } |> removeAttributes
        
        printfn "Removed %i attributes" attribCount     

    let cleanupSvg svg =
        let doc = XDocument.Parse(svg)
        printfn "Cleaning garbage namespaces from SVG"
        let c = redundantNamespaces |> Seq.map (fun x-> cleanGarbage (doc, x)) |> Seq.length
        printfn "Cleaned %i garbage namespaces" c
        cleanSvgGarbage (doc, svgNameSpace)
        doc.ToString()
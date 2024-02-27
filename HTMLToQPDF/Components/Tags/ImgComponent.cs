using HtmlAgilityPack;
using HTMLToQPDF.Components;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using System.Diagnostics;
using HTMLToQPDF.Utils;

namespace HTMLQuestPDF.Components.Tags
{
    internal class ImgComponent : BaseHTMLComponent
    {
        private readonly GetImgBySrc getImgBySrc;
        
        public ImgComponent(HtmlNode node, HTMLComponentsArgs args) : base(node, args)
        {
            this.getImgBySrc = args.GetImgBySrc;
        }

        protected override void ComposeSingle(IContainer container)
        {
            var src = node.GetAttributeValue("src", "");
            var style = node.GetAttributeValue("style", "");
            var width = 200.0f;
            var unitWidth = Unit.Point;
            var unitHeight = Unit.Point;
            var height = 100.0f;
            var unitMaxHeight = Unit.Point;
            var maxHeight = 100.0f;

            var img = getImgBySrc(src) ?? Placeholders.Image(200, 100);

            string[] styleSplit = style.Split(';');

            foreach (var element in  styleSplit)
            {
                string[] splitElement = element.Split(':');

                if (splitElement[0].Trim().ToLower().Equals("width"))
                {
                    string widthTemp = splitElement[1];
                    width = float.Parse(widthTemp.Substring(0, widthTemp.Length - 2).Replace(".", ","));
                    if (!splitElement[1].Contains("%"))
                    {
                        unitWidth = UnitUtils.ExtractUnit(widthTemp.Substring(widthTemp.Length - 2, 2));
                    }
                }

                if (splitElement[0].Trim().ToLower().Equals("height"))
                {
                    string heightTemp = splitElement[1];
                    height = float.Parse(heightTemp.Substring(0, heightTemp.Length - 2).Replace(".", ","));
                    if (!splitElement[1].Contains("%"))
                    {
                        unitHeight = UnitUtils.ExtractUnit(heightTemp.Substring(heightTemp.Length - 2, 2));
                    }
                }

                if (splitElement[0].Trim().ToLower().Equals("max - height"))
                {
                    string heightTemp = splitElement[1];
                    maxHeight = float.Parse(heightTemp.Substring(0, heightTemp.Length - 2).Replace(".", ","));
                    if (!splitElement[1].Contains("%"))
                    {
                        unitMaxHeight = UnitUtils.ExtractUnit(heightTemp.Substring(heightTemp.Length - 2, 2));
                    }
                }

                //Debug.WriteLine(element);
            }
            
            if (height == 0)
            {
                height = maxHeight;
                unitHeight = unitMaxHeight;
            }
            
            //Debug.WriteLine(height);
            //Debug.WriteLine(unitHeight);

            //style="width: 650px;max-height: 246px;"

            //max-width:100%
            //        width: 5.6979in
            //max - height:2.8541in
            //                width: 547px
            //max - height:274px
            
            //container.Image(img);
            //container.Height(height, unitHeight).Width(width, unitWidth).Image(img).FitUnproportionally();
            container.Height(height, unitHeight).Image(img).FitArea();
        }
    }
}
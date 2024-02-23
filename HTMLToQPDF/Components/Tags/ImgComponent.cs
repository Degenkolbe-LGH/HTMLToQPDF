using HtmlAgilityPack;
using HTMLToQPDF.Components;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using System.Diagnostics;

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
            var width = 0f;
            var height = 0f;
            var img = getImgBySrc(src) ?? Placeholders.Image(200, 100);

            string[] styleSplit = style.Split(';');

            foreach (var element in  styleSplit)
            {
                string[] splitElement = element.Split(':');

                if (splitElement[0].ToLower().Contains("width"))
                {
                     string widthtest = splitElement[1];
                    width = float.Parse(widthtest.Substring(0, widthtest.Length - 2));
                }

                if (splitElement[0].ToLower().Contains("height"))
                {
                    string heighttest = splitElement[1];
                    height = float.Parse(heighttest.Substring(0, heighttest.Length - 2));
                }

                Debug.WriteLine(element);
            }
            Debug.WriteLine(height);
            Debug.WriteLine(width);

            //style="width: 650px;max-height: 246px;"

            //max-width:100%
            //        width: 5.6979in
            //max - height:2.8541in
            //                width: 547px
            //max - height:274px

            //container.Image(img);
            container.Height(height, Unit.Point).Width(width, Unit.Point).Image(img).FitUnproportionally();
        }
    }
}
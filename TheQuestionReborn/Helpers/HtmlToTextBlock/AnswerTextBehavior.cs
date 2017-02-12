using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Documents;
using Windows.UI.Xaml.Media;

namespace TheQuestionReborn.Helpers.HtmlToTextBlock
{
    public class AnswerTextBehavior : Behavior<TextBlock>
    {
        protected override void OnAttached()
        {
            base.OnAttached();
            AssociatedObject.Loaded += OnAssociatedObjectLoaded;
            AssociatedObject.LayoutUpdated += OnAssociatedObjectLayoutUpdated;
        }

        protected override void OnDetaching()
        {
            base.OnDetaching();
            AssociatedObject.Loaded -= OnAssociatedObjectLoaded;
            AssociatedObject.LayoutUpdated -= OnAssociatedObjectLayoutUpdated;
        }

        private void OnAssociatedObjectLayoutUpdated(object sender, object o)
        {
            UpdateText();
        }

        private void OnAssociatedObjectLoaded(object sender, RoutedEventArgs routedEventArgs)
        {
            UpdateText();
            AssociatedObject.Loaded -= OnAssociatedObjectLoaded;
        }

        private void UpdateText()
        {
            try
            {
                if (string.IsNullOrEmpty(AssociatedObject?.Text)) return;
                var text = AssociatedObject.Text;
                AssociatedObject.Inlines.Clear();

                var htmlDoc = new HtmlDocument();
                htmlDoc.LoadHtml(text);
                ParseText(htmlDoc.DocumentNode.ChildNodes, AssociatedObject.Inlines);
            }
            catch (Exception e)
            {
                // ignored
            }
            finally
            {
                if (AssociatedObject != null)
                {
                    AssociatedObject.LayoutUpdated -= OnAssociatedObjectLayoutUpdated;
                    AssociatedObject.Loaded -= OnAssociatedObjectLoaded;
                }
            }

        }

        private void ParseText(IEnumerable<HtmlNode> descendants, InlineCollection inlines)
        {
            var currentInlines = inlines;
            foreach (var node in descendants)
            {
                switch (node.OriginalName)
                {
                    case "p":

                        if (inlines.Count != 0)
                        {
                            inlines.Add(new LineBreak());
                            inlines.Add(new LineBreak());
                        }

                        var paragraphSpan = new Span();
                        inlines.Add(paragraphSpan);
                        currentInlines = paragraphSpan.Inlines;
                        break;

                    case "a":
                        var link = new Hyperlink();
                        var href = node.Attributes["href"];
                        if (href != null)
                        {
                            try
                            {
                                link.NavigateUri = new Uri(href.Value);
                            }
                            catch (FormatException) { /* href is not valid */ }
                        }

                        link.Foreground = new SolidColorBrush(Colors.Blue);
                        inlines.Add(link);
                        currentInlines = link.Inlines;

                        break;

                    case "#text":
                        var span = new Span();
                        inlines.Add(span);
                        currentInlines = span.Inlines;
                        break;

                    case "b":
                    case "strong":
                        var bold = new Bold();
                        inlines.Add(bold);
                        currentInlines = bold.Inlines;
                        break;
                    case "br":
                        inlines.Add(new LineBreak());
                        break;
                    case "i":
                    case "em":
                        var italic = new Italic();
                        inlines.Add(italic);
                        currentInlines = italic.Inlines;
                        break;
                    case "u":
                        var underline = new Underline();
                        inlines.Add(underline);
                        currentInlines = underline.Inlines;
                        break;
                    case "ul":
                    case "div":
                        Span divSpan = new Span();
                        inlines.Add(divSpan);
                        currentInlines = divSpan.Inlines;
                        break;
                    case "text-image":
                    case "text-embed-image":
                    case "text-embed-video":
                    case "text-embed-og":

                        //var imgMessage = new Image();
                        //var bi = new BitmapImage();
                        //var url = node.Attributes["data-url"].Value;
                        //bi.UriSource = new Uri(@url, UriKind.RelativeOrAbsolute);

                        //imgMessage.Source = bi;
                        //imgMessage.Width = bi.PixelWidth;
                        //imgMessage.Height = bi.PixelWidth;
                        //var iuc = new InlineUIContainer() { Child = imgMessage };
                        //inlines.Add(iuc);
                        break;
                    case "li":
                    case "ol":
                        break;
                    default:
                        break;
                }

                if (node.HasChildNodes)
                {
                    ParseText(node.ChildNodes, currentInlines);
                }
                else
                {
                    currentInlines.Add(new Run { Text = node.InnerText });
                }
            }
        }
    }
}

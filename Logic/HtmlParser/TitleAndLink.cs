using System;

namespace NazdaqSearch.Logic.HtmlParser
{

    public class TitleAndLink {
        
        //Title of the traversalLinks
        public String Title { get; set; }
        public String Link { get; set; }
        public String Time { get; set; }

        public TitleAndLink(String title, String link, String time) {

            this.Title = title;
            this.Link = link;
            this.Time = time;

        }

    }

}
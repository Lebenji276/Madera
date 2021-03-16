namespace Madera
{
    internal class HttpStringContent
    {
        private string v1;
        private object utf8;
        private string v2;

        public HttpStringContent(string v1, object utf8, string v2)
        {
            this.v1 = v1;
            this.utf8 = utf8;
            this.v2 = v2;
        }
    }
}
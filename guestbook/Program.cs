// Emma Elmberg, HT2022

using System.Text.Json;

namespace posts {
    public class GuestBook {
        // Visar vilken json-fil som ska användas för att lagra författare och inlägg
        private string filename = @"guestbook.json";

        // Skapar ny lista med inlägg
        private List<Post> posts = new List<Post>();

        // Visar tidigare skrivna inlägg
        public GuestBook() {
            if(File.Exists(@"guestbook.json")==true) {
                string jsonString = File.ReadAllText(filename);
                posts = JsonSerializer.Deserialize<List<Post>>(jsonString);
            }
        }

        // Lägga till inlägg
        public Post addPost (Post post) {
            posts.Add(post);
            marshal();
            return post;
        }

        // Ta bort inlägg
        public int deletePost(int index) {
            posts.RemoveAt(index);
            marshal();
            return index;
        }

        public List<Post> getPosts() {
            return posts;
        }

        // Numrera objekten och spara till filen
        private void marshal() {
            var jsonString = JsonSerializer.Serialize(posts);
            File.WriteAllText(filename, jsonString);
        }
    }

    // Används för att hämta författare och inlägg
    public class Post {

        private string post_author;
        public string Post_author {
            set {this.post_author = value;}
            get {return this.post_author;}
        }

        private string post_text;
        public string Post_text {
            set {this.post_text = value;}
            get {return this.post_text;}
        }
    }

    class Program {
        static void Main(string[] args) {
            GuestBook guestbook = new GuestBook();
            
            int i=0;

            while(true) {
                Console.Clear();Console.CursorVisible = false;
                Console.WriteLine("Välkommen till Emmas gästbok\n");

                Console.WriteLine("1. Skriv ett inlägg");
                Console.WriteLine("2. Ta bort ett inlägg \n");
                Console.WriteLine("X. Avsluta\n");

                i=0;
                foreach(Post post in guestbook.getPosts()) {
                    // Så här ska inläggen se ut vid publicering
                    Console.WriteLine("[" + i++ + "] " + post.Post_author + " - " + post.Post_text);
                }

                int inp = (int) Console.ReadKey(true).Key;
                switch (inp) {
                    case '1':
                        Console.CursorVisible = true;
                        Post obj = new Post();

                        Console.Write("\nFörfattare: ");
                        string author = Console.ReadLine();
                        obj.Post_author = author;
                        
                        // Enklare felhantering för tomma fält
                        if(string.IsNullOrWhiteSpace(author)) {
                            Console.Write("Sträng får ej vara tom! Vänligen tryck på valfri tangent för att börja om.");
                            Console.ReadKey(true);
                        } else {
                        Console.Write("\nHälsning: ");
                        string text = Console.ReadLine();
                        obj.Post_text = text;
                            if(string.IsNullOrWhiteSpace(text)) {
                                Console.Write("Fält får ej vara tomt! Vänligen tryck på valfri tangent för att börja om.");
                                Console.ReadKey(true);
                            } else
                            guestbook.addPost(obj);
                        }
                        break;
                    case '2':
                        Console.CursorVisible = true;
                        Console.Write("\nAnge index att radera: ");
                        string index = Console.ReadLine();

                        // Enklare felhantering för tomma fält
                        if(string.IsNullOrWhiteSpace(index)) {
                            Console.Write("Fält får ej vara tomt! Vänligen tryck på valfri tangent för att börja om.");
                            Console.ReadKey(true);
                        } else {
                            guestbook.deletePost(Convert.ToInt32(index));
                        }
                        break;
                    case 88:
                        Environment.Exit(0);
                        break;
                }
            }
        }
    }
}
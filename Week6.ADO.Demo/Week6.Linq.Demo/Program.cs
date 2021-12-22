using System;
using System.Collections.Generic;
using System.Linq;
using Week6.Linq.Demo.Entities;

namespace Week6.Linq.Demo
{
    internal class Program
    {
        #region DATA
        public static List<Book> GetBooks()
        {
            var books = new List<Book>()
            {
                new Book() {
                    Id = 1,
                    Title = "Harry Potter e la pietra filosofale",
                    Genre = "Fantasy",
                    PublishYear = 2000,
                    Pages = 149,
                    AuthorId = 1
                },
                new Book() {
                    Id = 2,
                    Title = "Una terra promessa",
                    Genre = "Autobiografia",
                    PublishYear = 2020,
                    Pages = 559,
                    AuthorId = 2
                },
                new Book(){
                    Id = 3,
                    Title = "Uno nessuno centomila",
                    Genre = "Monologo",
                    PublishYear = 1930,
                    Pages = 120,
                    AuthorId = 3
                },
                new Book(){ 
                    Id = 4,
                    Title = "Robinson Crusoe",
                    Genre = "Romanzo",
                    PublishYear = 1880,
                    Pages = 230,
                    AuthorId = 4
                },
                new Book() {
                    Id = 5,
                    Title = "Harry Potter e il calice di fuoco",
                    Genre = "Fantasy",
                    PublishYear = 2010,
                    Pages = 400,
                    AuthorId = 1
                }
            };

            return books;
        }

        public static List<Author> GetAuthors()
        {
            var authors = new List<Author>()
            {
                new Author() { 
                    Id = 1,
                    FirstName = "J. K",
                    LastName = "Rowling",
                    Nationality = "Inglese"
                },
                new Author() { 
                    Id = 2,
                    FirstName = "Barack",
                    LastName = "Obama",
                    Nationality = "Americana"
                },
                new Author() { 
                    Id = 3, 
                    FirstName = "Luigi",
                    LastName = "Pirandello",
                    Nationality = "Italiana"
                },
                new Author()
                {
                    Id = 4,
                    FirstName = "Daniel",
                    LastName = "Defoe",
                    Nationality = "Inglese"
                }
            };
            
            return authors;
        }

        public static List<Review> GetReviews()
        {
            var reviews = new List<Review>()
            {
                new Review() {
                    Id = 1,
                    BookId = 1,
                    Vote = 5,
                    Description = "Il miglior libro del mondo"
                },
                new Review(){
                    Id = 2,
                    BookId = 2,
                    Vote = 3,
                    Description = "Un po' noioso"
                }
            };

            return reviews;
        }
        #endregion
        static void Main(string[] args)
        {
            QuerySyntax();
            LambdaSyntax();
        }

        public static void LambdaSyntax()
        {
            var books = GetBooks();
            var authors = GetAuthors();
            var reviews = GetReviews();

            //libri pubblicati dal 2000 in poi e con più di 200 pagine
            var booksOver2000 = books.Where(x => x.PublishYear >= 2000 && x.Pages > 200);
                                    //.Select(x => x.Title);
            foreach (var item in booksOver2000)
            {
                Console.WriteLine($"{item.Id} - {item.Title} - {item.PublishYear}");
            }
            //Titoli dei libri con più di 200 pagine
            var booksLong = books.Where(book => book.Pages > 200).Select(book => book.Title);
            foreach (var item in booksLong)
            {
                Console.WriteLine($"Title: {item}");
            }

            //Ordina recensioni per voto e per Id
            var orderedReview = reviews.OrderBy(r => r.Vote).ThenByDescending(r => r.Id);
            foreach (var rev in orderedReview)
            {
                Console.WriteLine($"{rev.Id} - {rev.Vote} - {rev.Description}");
            }

            //Libro con il massimo numero di pagine
            var bookMaxPages = books.Where(x => x.Pages == books.Max(b => b.Pages)).First();
            Console.WriteLine($"Il libro con il massimo numero di pagine è: {bookMaxPages.Title}");

            //Raggruppamento degli autori per nazionalità
            var authorsGroup = authors.GroupBy(x => x.Nationality)
                               .Select(x => new
                               {
                                   Key = x.Key,
                                   Authors = x.Select(x => x)
                               });
            foreach (var author in authorsGroup)
            {
                Console.WriteLine(author.Key);
                foreach (var item in author.Authors)
                {
                    Console.WriteLine($"{item.Id} - {item.FirstName} - {item.LastName}");
                }
            }

            var sumPagesGenre = books.GroupBy(book => book.Genre)
                                    .Select(bookForGenre => new
                                    {
                                        Genre = bookForGenre.Key,
                                        SumOfPages = bookForGenre.Sum(x => x.Pages)
                                    });
        }

        public static void QuerySyntax()
        {
            var books = GetBooks();
            var authors = GetAuthors();
            var reviews = GetReviews();

            //libri pubblicati dal 2000 in poi

            //SENZA LINQ
            //var booksOver2000 = new List<Book>();
            //foreach(var book in books)
            //{
            //    if(book.PublishYear > 2000)
            //    {
            //        booksOver2000.Add(book);
            //    }
            //}
            //CON LINQ
            var booksOver2000 = from book in books
                                where book.PublishYear >= 2000
                                select book;
            foreach (var book in booksOver2000)
            {
                Console.WriteLine($"Title: {book.Title} - Publish Year {book.PublishYear}");
            }

            //Titoli dei libri con più di 200 pagine
            var titleBookLong = from book in books
                                where book.Pages >= 200
                                select book.Title;
            foreach (var book in titleBookLong)
            {
                Console.WriteLine($"Title: {book}");
            }

            //Ordina recensioni per voto e per Id
            var orderedReview = from review in reviews
                                orderby review.Vote ascending, review.Id descending
                                select review;
            foreach(var review in orderedReview)
            {
                Console.WriteLine($"ID: {review.Id} - Voto: {review.Vote}");
            }

            //Libro con il massimo numero di pagine
            var booksMaxPages = from book in books
                               where book.Pages == books.Max(b => b.Pages)
                               select book;
            var singleBookMaxPage = booksMaxPages.First();
            Console.WriteLine("Il libro con il massimo numero di pagine è {0}", singleBookMaxPage.Title);

            //Raggruppamento degli autori per nazionalità
            var authorsPerNationality = from author in authors
                                        group author by author.Nationality into AuthorNationality
                                        select AuthorNationality;

            //Inglese - J.K. Rowling
            //        - Daniel Defoe
            //Americano - Barack Obama
            //Italiano - Luigi Pirandello
            foreach (var authorGrouped in authorsPerNationality)
            {
                Console.WriteLine(authorGrouped.Key);
                foreach (var aut in authorGrouped)
                {
                    Console.WriteLine($"ID: {aut.Id} - Nome: {aut.FirstName} - Cognome {aut.LastName}");
                }
            }

            //Raggruppo i libri per genere e sommo il numero di pagine
            var sumPagesGenre = from book in books
                                group book by book.Genre into groupListGenre
                                select new
                                {
                                    Genre = groupListGenre.Key,
                                    TotalPages = groupListGenre.Sum(book => book.Pages)
                                };
            //Fantasy - Tot. 549
            //Romanzo - Tot. 200
            //Autobiografia - Tot. 120
            foreach(var item in sumPagesGenre)
            {
                Console.WriteLine($"{item.Genre} - Tot. {item.TotalPages}");
            }
                                        


        }
    }
}

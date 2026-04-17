
This project is to demonstrate how to create Nested MongoDb documents, up to the 3rd level, with the MongoDB NET C# driver.

Unfortunately, the 3rd level of nested documents does not get sent to the DB.

Serilog out show this on the insert operation:
	Command started insert TheCodeBuzz { "insert" : "LibraryUsers", "ordered" : true, "$db" : "TheCodeBuzz", "lsid" : 
	{ "id" : { "$binary" : { "base64" : "1rbZvuTfRXOqoLwQftrpXQ==", "subType" : "04" } } }, 
	"documents" : 
	[{ "_id" : { "$oid" : "69e22dcac06b765666fcda19" }, "UserId" : 999999, "Books" : 
	[
	 { "_id" : { "$oid" : "69e22dcac06b765666fcda1a" }, "BookTitle" : "Old Yeller", "Price" : 11 }, 
	 { "_id" : { "$oid" : "69e22dcac06b765666fcda1c" }, "BookTitle" : "Moby Dick", "Price" : 15 }] }
	] }
	Command succeeded insert TheCodeBuzz 10.1227 { "n" : 1, "ok" : 1.0 }

Tried to do manually mapping to "force" the 3rd level to be "seen", but it didn't make a difference.

Posted question on StackOverflow
https://stackoverflow.com/questions/79927435/mongodb-net-driver-failing-to-create-nested-documents-3-levels-deep/79927471#79927471
Answered by Younk Shun.
I forgot to declare the Book's member "List<Review> Reviewers" as 'public'. DOH!


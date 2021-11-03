# Filestransformer
PROBLEM OBJECTIVE:
Please write a program to process text files. Each file can belong to one of three groups: {group1, group2, group3}. Each group can have an arbitrary number of files associated with it, represented as:

group1.work1.txt
group1.work2.txt
group1.work3.txt
group2.work1.txt
group2.work2.txt
group3.work1.txt
...

As you process each file, you create a processed version by applying a simple transformation (described below). You must NOT read the entire contents of the file stream into memory. Once the processing completes, you finalize a new processed version of a file, as follows:

group1.work1.out
group1.work2.out
...
group3.work1.out

ASSUMPTIONS/REQUIREMENTS:
-	The processed version of each file is simply the lowercase contents of the original file. 
-	An ideal class design will allow future extensibility by enabling others to write new transformations. Lowercase is one built-in transformation.
-	You can process AT MOST three files simultaneously. An ideal class design shoud allow changing this number easily.
-	New files can be added at any time during the lifetime of your program. 
-	You could start with an empty directory and suddenly see a rush of files to be processed from one or more groups. Your program must accommodate this dynamic behavior. Similarly, while you're processing, new files can be added at runtime.
-	Files will never be removed. They will only be added.
-	There's no support for process cancellation, meaning once you've started processing a file, you complete it.
 
CODE HYGIENE:
-	You must write this code as if it is clean, production quality. Your code must be testable thoroughly with as little side-effect as possible.
-	It would be best to think through critical areas of the code and honor the open/closed principle where it makes sense. (see the second point in assumptions)
-	You must write at least one unit test to demonstrate testability.
-	Create a new private git repo in Github, and follow standard source structures
-	You should accommodate and argue design decisions in code comments (if you believe in them strongly).

## Tool usage
### Syntax
FileTransformer.exe [-p <Int32>] [-i <String>] [-o <String>] [-f <Int32>] [-e <Int32>] <br>
-p : maximum number of transformations allowed PER GROUP in an epoch, e.g., 3 <br>
-i : input directory where the file transformations will reside, e.g., c:\users\rags\appdata\localtemp\inputs <br>
-o : output directory where the completed file transformations will reside e.g., c:\users\rags\appdata\localtemp\outputs <br>
-f : number of bytes for file transformation to transform at a time e.g., 1024 <br>
-e : file encoding type of transformed file e.g., utf8 <br>
 
This tool uses [Microsoft Coyote](https://microsoft.github.io/coyote/), formerly known as PSharp to achieve the results


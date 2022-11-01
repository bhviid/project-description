**GitInsight**  is an application made for the course *Analysis, Design and Software Architecture* during autumn 2022 as part of the BSc in Software Development program at IT-University of Copenhagen. The project runs between week 43 and week 49. 

## Functional requirements
### Software
* GitInsight is an application that allow users to get insight in the commit frequency and commit authors in their git repositories/projects locally. (week 43) 
*  GitInsight should be a C#/.Net Core application. (week 43)
* Given a path to a repository as a command-line argument, the application should print the textual output to standard out.  (week 43)
* GitInsight supports two modes: 
    * *frequency*. lists the number of frequency per day. 
    * *commit author*. lists the number of commits per day per author.  (week 43)

* Store the results / analyze from running "frequency" and "author" on git repositories in a persistent database. (week 44)
   * Store information about which repositories were analyzed at which state. (week 44)
   *  The database should store most recent commits. Database entries should be re-analyzed if a previous entry is not updated, but if the entry is up to date, the      application should just output from the readily available data. (week 44)

### Development
* The program should be developed iteratively, expanding the the software requirements each week. Requirements are subject to change throughout the development period. (week 43)
* Should be developed in an agile and test-driven manner. (week 43)

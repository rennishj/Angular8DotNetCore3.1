# Angular8WithDotNetCore3.1
This is a sample application built in Asp.Net Core 3.1 and Angular 8 framework.
Angular SPA is in the directory Assignmentt-SPA and Web api in Assignment.API.
The back end database used is sql server and ORM is Dapper 2.0.35.

Steps to Run Assignment.API
=================================
1. Make sure you have Dot Net Core 3.1 and sql server 2008R2 or later installed.
2. Open the solution file located in the project root in VS 2019 or 2017
3. Right click Assigment.API and set that as start up project after building the solution
4. Run the API project and keep it running for the SPA application.
5. Run the dml_and_ddl.sql file from  the TSqlScripts directory of the Assignment.API project in sql server management studio.

Steps to run Angular SPA
===========================
1. Make sure you have NPM version 6.2 or later installed. This is specifically built against 6.13
2. OPen Assignment.SPA is a code editor like Vs code
3.Open up the terminal and type npm install to  download the dependencies installed in package.json file
4.Type ng serve -o to open  up abrowser in  port 4200

Test files for LISP Code and Enrollment File functionality 
===================================================================
1. Burst enrollment file is tested aginst the file format used in the Assigment.API\TestFiles\enrollment.csv
2. Vallidate LISP code is testd against the  valid_lisp_code.txt, invalid_lisp_code.txt files.
3. Enrollment files will be bursted into wwwroot folder.





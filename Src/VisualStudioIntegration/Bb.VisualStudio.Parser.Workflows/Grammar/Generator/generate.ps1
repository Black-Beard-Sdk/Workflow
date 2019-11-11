# antlr-4.7-complete.jar must be preset 
# http://www.antlr.org/download/antlr-4.7-complete.jar


cp ..\..\..\..\Black.beard.Workflow.AntlrParser\Parser\Grammar\*.g4 .

java.exe -jar antlr-4.7-complete.jar -Dlanguage=CSharp WorkflowLexer.g4 -visitor -no-listener -package Bb.VisualStudio.Parser.Workflows.Grammar
java.exe -jar antlr-4.7-complete.jar -Dlanguage=CSharp WorkflowParser.g4 -visitor -no-listener -package Bb.VisualStudio.Parser.Workflows.Grammar

rm *.g4
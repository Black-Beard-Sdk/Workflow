# antlr-4.7-complete.jar must be preset 
# http://www.antlr.org/download/antlr-4.7-complete.jar

java.exe -jar antlr-4.7-complete.jar -Dlanguage=CSharp WorkflowLexer.g4 -visitor -no-listener -package Bb.Workflows.Parser
java.exe -jar antlr-4.7-complete.jar -Dlanguage=CSharp WorkflowParser.g4 -visitor -no-listener -package Bb.Workflows.Parser
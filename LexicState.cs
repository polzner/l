namespace Syntax
{
     public enum LexicState 
     {
         Start,
         Number,
         Delimiter,
         Final, 
         Word, 
         Error, 
         Assign,
         Comment
     }
}
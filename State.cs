namespace Syntax
{
     public enum State 
     {
         Start,
         Number,
         Delimiter,
         Final, 
         Word, 
         Error, 
         Assign
     }
}
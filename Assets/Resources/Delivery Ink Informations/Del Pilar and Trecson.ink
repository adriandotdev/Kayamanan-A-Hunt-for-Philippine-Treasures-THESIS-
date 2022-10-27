VAR isAllVisited = false
VAR count = -1
-> main

=== main ===
    Thank you for bringing this to me. I actually need this to display it on my house. In relate to that, I know some of our national heroes that can be relate to this. Which one you want to go tackle first?
    -> items
    + [Next]
-> END

=== items ===
    ~ count = count + 1
    * [Marcelo H. Del Pilar]
    Marcelo H. Del Pilar known by his pen name <b>Plaridel</b>. He was a Filipino <b>writer</b>, <b>lawyer</b>, <b>journalist</b>, and <b>freemason</b>. He was born on <b>August 30, 1850</b>, and brought up in <b>Bulakan, Bulacan</b>. 
    + + [Next]
    Del Pilar also a contributor as an editor for the propaganda movement newspaper <b>La Solidaridad</b>. 
    + + + [Next]
    He was sent to <b>Spain</b> because of his anti-friar activities. (<b>Friars</b> are members of any of certain religious orders of men, or often called <b>priests</b>), and lastly, he died on <b>July 4, 1986</b> at aged of 45.
    -> items
    * [Gregorio Del Pilar]
    Gregorio Del Pilar or <b>Gregorio Hilario del Pilar y Sempio</b> was born on November 14, 1875 and died in <b>December 2, 1899</b> at aged of 24. He was one of the youngest generals in the revolutionary army during the Philippine-American War. 
    + + [Next]
    Because of his youth, he became known as the <b>Boy General</b> or <b>Goyo</b>. Lastly, he was the nephew of Marcelo H. Del Pilar.
    -> items
    + {count >= 2} [Next]
    That's all. If you want to recap some of their information, just let me know.
        + + [Exit]
        -> DONE
-> END

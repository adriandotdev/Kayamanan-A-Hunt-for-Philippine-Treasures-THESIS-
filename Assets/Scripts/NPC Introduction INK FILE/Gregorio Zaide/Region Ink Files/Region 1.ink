-> main

VAR isFirstIterated = false
VAR numberOfChoices = 0

=== main ===
{ not isFirstIterated: {~Hello! How can I help you|Hello, do you want to know some of the heroes?|Hello there! What can I do for you?} }
    
{ isFirstIterated && numberOfChoices != 2: Lets tackle about next national hero.}
    
* [{isFirstIterated: Okay | Do you know Juan Luna?}]
    -> information.JUAN_LUNA
* [{isFirstIterated: Okay | Do you know Gabriela Silang?}]
    -> information.GABRIELA_SILANG
* -> 
    That's all! If you want to recap some of their information, just let me know.
    + + [Next]
-> END

=== information ===
    = JUAN_LUNA
    {isFirstIterated: The next Filipino hero is Juan Luna. | Yes! He is one of the notable heroes in the Philippines.}
    ~ isFirstIterated = true
    ~ numberOfChoices += 1
    + [Next]
        He was born as Juan Novicio Luna on October 23, 1857 at Bardoc, Ilocos Norte.
        + + [Next]
            He is a painter, a sculptor, and a political activist.
            + + + [Next]
                He also well-know about his painting named "Spolarium". Have you seen it in the museum?
                + + + + [Yes]
                    -> simple_question
                + + + + [Not yet]
                    I see. You can see it for more details in the museum.
                    -> end_luna
                -> DONE

    = GABRIELA_SILANG
    {isFirstIterated: The next hero that I know in region 1 is Gabriela Silang | Yes! She was born as Maria Josefa Gabriela Carino de Silang on March 19, 1731 at Santa, Ilocos Sur.}
    ~ isFirstIterated = true
    ~ numberOfChoices += 1
    + [Next]
    She is also known as female leader for the Ilocano independence movement against Spain.
        + + [Next]
        She is also a wife of Diego Silang.
        + + + [Next]
        -> main
    
    

    
-> DONE

=== simple_question ===
That's great! for a little recap. When did he painted it?
+ [1844]
    Correct! Seems like you will ace the assessment at the event.
    -> end_luna
+ [1744]
    I think you forgot it. You can check it inside the museum for more details.
    -> end_luna

=== end_luna ===
    + [Next]
    Also, the cause of his death is cardiac arrest at British, Hongkong on December 7, 1899.
        + + [Exit]
            -> main
-> main
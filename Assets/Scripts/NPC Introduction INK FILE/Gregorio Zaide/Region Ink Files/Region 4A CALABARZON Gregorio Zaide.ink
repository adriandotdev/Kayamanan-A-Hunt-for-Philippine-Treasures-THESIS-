-> main
VAR count = -1

=== main ===
    #text: value
    Hello. How may I help you? 
    + [Do you know Mabini and Aguinaldo?]
    Yes! Which one you want to discuss first?
    -> info
-> END

=== info ===
    ~ count = count + 1
    * [Apolinario Mabini]
    Aplinario Mabini or <b>Apolinario Mabini y Maranan</b> is a Filipino Revolutionary leader, educator, lawyer, and a statesman who served first as a legal and constitutional adviser to the Revolutionary Government, and then as the first <b>Prime Minister of the Philippines</b>.
    + + [Next]
    He is regarded as the <b>"Utak ng Himagsikan"</b> or <b>"brain of the revolution</b>
    + + +[Next]
    He lost both of his legs due to Polio.
    -> info
    * [Emilio Aguinaldo]
    Emilio Aguinaldo or <b>Emilio Aguinaldo y Famy</b> is from <b>Kawit, Cavite</b>. He is a Filipino revolutionary, statesman, and military leader who is officially recognized as the <b>first and the youngest president of the Philippines</b>.
    + + [Next]
    He led Philippine forces first against Spain in the Philippine Revolution (1896-1898), then in the Spanish-American War (1898), and finally against the United States during the Philippine-American War (1899-1901).
    + + + [Next]
    Emilio Aguinaldo died due to old age.
    -> info
     + {count >= 2} [Next Last of Emilio]
     That's all. If you want to recap some of their information, just let me know.
        + + [Exit]
        -> DONE
-> END
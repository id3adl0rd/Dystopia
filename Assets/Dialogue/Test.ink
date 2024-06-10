EXTERNAL startQuest(string text)
EXTERNAL closeDialogue()
-> main

=== main ===
Ну и чего тебе?
    + [Мне нужно задание]
        -> StartQuest()
    + [Кто ты?]
        -> about()
    + [Прощай]
        ~ closeDialogue()
        -> goodbye()

=== StartQuest() ===
{startQuest("")}
-> END

=== about() ===
Я местный представитель гильдии наемников. Могу порой подкинуть работенку.
-> END

=== goodbye() ===
Ладно, прощай.
-> END

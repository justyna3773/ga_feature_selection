## Implementacja klasycznego algorytmu genetycznego z funkcja przystosowania opartą o średnie wartości cech 

Zastosowana funkcja przystosowania składa się z dwóch części: 
similarity score, jeśli cecha danej próbki mieści się w przedziale średnia +- odchylenie stand. dla danej cechy dodawany jest punkt, w przeciwnym razie dodawane jest 0. 
dissimilarity score, jeśli cecha danej próbki mieści się w przedziale średnia +- odchylenie stand. (dla danej cechy u innej osoby) dodawane jest 0, w przeciwnym razie dodawane jest 1. 
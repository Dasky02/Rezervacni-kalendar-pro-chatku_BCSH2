# Rezervacni-kalendar-pro-chatku_BCSH2
Tento projekt je semestrální prací pro předmět BCSH2. Cílem projektu je vytvořit jednoduchou databázovou aplikaci pro správu rezervací chatky, která zahrnuje přihlašování uživatelů, zobrazení obsazenosti a blokování registrace na obsazené časy.

Funkcionality

	•	Přihlášení uživatele: Uživatelé se musí přihlásit, aby mohli provádět rezervace a vidět dostupné časy.
	•	Zobrazení obsazenosti: Uživatelé vidí kalendář s dostupnými a obsazenými časy.
	•	Blokování registrace na obsazené časy: Jakmile je časový slot obsazen, další uživatelé se nemohou registrovat na stejný čas.
	•	Možnost editace rezervace: Uživatelé mohou upravit nebo zrušit své rezervace, pokud ještě nenastaly.

 Entity a vztahy

Projekt bude obsahovat minimálně tři typy entit:

	1.	Uživatel:
	•	ID
	•	Jméno
	•	Email
	•	Heslo (uloženo bezpečně)
	2.	Chatka:
	•	ID
	•	Název chatky
	•	Popis
	3.	Rezervace:
	•	ID
	•	Uživatel (vztah na entitu Uživatel)
	•	Chatka (vztah na entitu Chatka)
	•	Datum a čas rezervace

Plán vývoje

	1.	Základní nastavení projektu – Vytvoření základní struktury projektu a propojení s databází.
	2.	Přihlašování uživatelů – Implementace autentizace pomocí ASP.NET Identity.
	3.	Zobrazení kalendáře – Vytvoření kalendáře pro zobrazení dostupných a obsazených časů.
	4.	Rezervace a blokování termínů – Uživatelé mohou rezervovat chatky a termíny budou blokovány pro ostatní uživatele.
	5.	Editace rezervací – Uživatelé mohou upravit nebo zrušit své rezervace.
	6.	Testování a ladění – Ověření funkčnosti všech částí projektu.
	7.	Prezentace projektu – Příprava na prezentaci projektu do 10. 11. 2024.

Autor

Lukáš Roubíček

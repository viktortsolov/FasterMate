# :thought_balloon: Introduction
**FasterMate** is a web application, which provides social media functionality. Here you can meet with new people, have a chat and the main purpose of the application - express your impressions from the trips you have had in the past. You can buy plane tickets from other people and add photos to your profile.

# :link: Link
https://fastermate.azurewebsites.net/

# 📃 Description
The web application has the following controllers:

**Home** - this controller consists of a Welcome Page, Timeline Page and Search Page. The **Timeline** page illustrates the newest uploaded posts from the different users. From the **Search** page you can search for new friends by *first name, last name and username.*

**Profile** - everything, which is related to your personal information is here. From this controller you can access **UserProfile** page, where there is personal info for each user like *birthdate, gender, bio, etc.* Moreover, you can see the recent posts of the user on their profile. However, if you want to change something about you, you have an **Edit** page. From this page you can *Follow and Unfollow* the user.

**Post** - you can add photo posts, where you can share your best experiences with the others. You can **Add Post** and mark the location that you have been. The others can *Like* and *Comment* your posts. You can delete *comments* on your post and delete your *posts* as well.

**Offer** - FasterMate is not only a social media, but a place where you can bargain for different *plane tickets* to different countries. You can **Add Ticket** and make money from it. However, you can book flights as well when you choose the best destination on **Book a Flight**. You can keep an eye on the tickets you have in **My Flights** and that will help you to not miss your flight.

**Group** - from there you can add groups to chat with your friends or you can bargain for the ticket you had chosen with the owner of it. You can express your emotions from the places you have been to and give recommendations. You can only add profiles that you are following. You can edit *the name and the image.* Only the **owner** of the group can edit it and add other people.

**Administrator Area** - from the administrator area *(if you are administrator)* you can **edit different profile roles**. You can **delete offers** that have expired throughout the time. If you think a *post* inappropriate - you can also **delete it**.

**Api** - here you can see **information** about all the profiles. *(First Name, Last Name, Gender, Email, Username)*

# ⚙ Technologies & Resources
 - ASP.NET Core 6.0
 - AdminLTE Bootstrap Template
 - NUnit
 - HTMLSanitizer
 - SignalR

# 🗃️ Database
![FasterMate-DB](https://user-images.githubusercontent.com/75324909/163596161-9a4447ab-b407-4294-88a0-882650be69fd.png)

# 🧪 Unit Tests
![tests](https://user-images.githubusercontent.com/75324909/163595527-1650c6c9-d008-49dd-b699-34a70606c90c.png)

# :warning: Local environment
When you open the web application on your machine, go to **Package Manager Console** and type **dotnet watch run** to start it.

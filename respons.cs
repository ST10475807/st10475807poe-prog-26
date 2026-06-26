using System.Collections;

namespace demo
{//start of namespace
    public class respond
    {//start of class
        public respond(ArrayList reply, ArrayList ignore)
        {//start of constructor

            answers(reply);
            words(ignore);


        }//end of constructor

        //method to store the list of words
        private void words(ArrayList ignoring)
        {//
         //ignoring questions
            ignoring.Add("a");
            ignoring.Add("about");
            ignoring.Add("above");
            ignoring.Add("across");
            ignoring.Add("after");
            ignoring.Add("afterwards");
            ignoring.Add("again");
            ignoring.Add("against");
            ignoring.Add("all");
            ignoring.Add("almost");
            ignoring.Add("alone");
            ignoring.Add("along");
            ignoring.Add("already");
            ignoring.Add("also");
            ignoring.Add("although");
            ignoring.Add("always");
            ignoring.Add("am");
            ignoring.Add("among");
            ignoring.Add("amongst");
            ignoring.Add("amount");
            ignoring.Add("an");
            ignoring.Add("and");
            ignoring.Add("another");
            ignoring.Add("any");
            ignoring.Add("anyhow");
            ignoring.Add("anyone");
            ignoring.Add("anything");
            ignoring.Add("anyway");
            ignoring.Add("anywhere");
            ignoring.Add("are");
            ignoring.Add("around");
            ignoring.Add("as");
            ignoring.Add("at");
            ignoring.Add("back");
            ignoring.Add("be");
            ignoring.Add("became");
            ignoring.Add("because");
            ignoring.Add("become");
            ignoring.Add("becomes");
            ignoring.Add("becoming");
            ignoring.Add("been");
            ignoring.Add("before");
            ignoring.Add("beforehand");
            ignoring.Add("behind");
            ignoring.Add("being");
            ignoring.Add("below");
            ignoring.Add("beside");
            ignoring.Add("besides");
            ignoring.Add("between");
            ignoring.Add("beyond");
            ignoring.Add("both");
            ignoring.Add("but");
            ignoring.Add("by");
            ignoring.Add("can");
            ignoring.Add("cannot");
            ignoring.Add("could");
            ignoring.Add("did");
            ignoring.Add("do");
            ignoring.Add("does");
            ignoring.Add("doing");
            ignoring.Add("done");
            ignoring.Add("down");
            ignoring.Add("during");
            ignoring.Add("each");
            ignoring.Add("either");
            ignoring.Add("else");
            ignoring.Add("elsewhere");
            ignoring.Add("enough");
            ignoring.Add("etc");
            ignoring.Add("even");
            ignoring.Add("ever");
            ignoring.Add("every");
            ignoring.Add("everyone");
            ignoring.Add("everything");
            ignoring.Add("everywhere");
            ignoring.Add("except");
            ignoring.Add("few");
            ignoring.Add("first");
            ignoring.Add("for");
            ignoring.Add("former");
            ignoring.Add("formerly");
            ignoring.Add("from");
            ignoring.Add("further");
            ignoring.Add("had");
            ignoring.Add("has");
            ignoring.Add("have");
            ignoring.Add("having");
            ignoring.Add("he");
            ignoring.Add("hence");
            ignoring.Add("her");
            ignoring.Add("here");
            ignoring.Add("hereafter");
            ignoring.Add("hereby");
            ignoring.Add("herein");
            ignoring.Add("hereupon");
            ignoring.Add("hers");
            ignoring.Add("herself");
            ignoring.Add("him");
            ignoring.Add("himself");
            ignoring.Add("his");
            ignoring.Add("how");
            ignoring.Add("however");
            ignoring.Add("i");
            ignoring.Add("if");
            ignoring.Add("in");
            ignoring.Add("indeed");
            ignoring.Add("inside");
            ignoring.Add("instead");
            ignoring.Add("into");
            ignoring.Add("is");
            ignoring.Add("it");
            ignoring.Add("its");
            ignoring.Add("itself");
            ignoring.Add("last");
            ignoring.Add("later");
            ignoring.Add("latter");
            ignoring.Add("latterly");
            ignoring.Add("least");
            ignoring.Add("less");
            ignoring.Add("lot");
            ignoring.Add("many");
            ignoring.Add("may");
            ignoring.Add("me");
            ignoring.Add("meanwhile");
            ignoring.Add("might");
            ignoring.Add("more");
            ignoring.Add("moreover");
            ignoring.Add("most");
            ignoring.Add("mostly");
            ignoring.Add("much");
            ignoring.Add("must");
            ignoring.Add("my");
            ignoring.Add("myself");
            ignoring.Add("name");
            ignoring.Add("namely");
            ignoring.Add("neither");
            ignoring.Add("never");
            ignoring.Add("nevertheless");
            ignoring.Add("next");
            ignoring.Add("no");
            ignoring.Add("nobody");
            ignoring.Add("none");
            ignoring.Add("noone");
            ignoring.Add("nor");
            ignoring.Add("not");
            ignoring.Add("nothing");
            ignoring.Add("now");
            ignoring.Add("nowhere");
            ignoring.Add("of");
            ignoring.Add("off");
            ignoring.Add("often");
            ignoring.Add("on");
            ignoring.Add("once");
            ignoring.Add("one");
            ignoring.Add("only");
            ignoring.Add("or");
            ignoring.Add("other");
            ignoring.Add("others");
            ignoring.Add("otherwise");
            ignoring.Add("ought");
            ignoring.Add("our");
            ignoring.Add("ours");
            ignoring.Add("ourselves");
            ignoring.Add("out");
            ignoring.Add("outside");
            ignoring.Add("over");
            ignoring.Add("own");
            ignoring.Add("part");
            ignoring.Add("per");
            ignoring.Add("perhaps");
            ignoring.Add("please");
            ignoring.Add("put");
            ignoring.Add("rather");
            ignoring.Add("re");
            ignoring.Add("same");
            ignoring.Add("see");
            ignoring.Add("seem");
            ignoring.Add("seemed");
            ignoring.Add("seeming");
            ignoring.Add("seems");
            ignoring.Add("several");
            ignoring.Add("she");
            ignoring.Add("should");
            ignoring.Add("show");
            ignoring.Add("side");
            ignoring.Add("since");
            ignoring.Add("so");
            ignoring.Add("some");
            ignoring.Add("somehow");
            ignoring.Add("someone");
            ignoring.Add("something");
            ignoring.Add("sometime");
            ignoring.Add("sometimes");
            ignoring.Add("somewhere");
            ignoring.Add("still");
            ignoring.Add("such");
            ignoring.Add("take");
            ignoring.Add("than");
            ignoring.Add("that");
            ignoring.Add("the");
            ignoring.Add("their");
            ignoring.Add("theirs");
            ignoring.Add("them");
            ignoring.Add("themselves");
            ignoring.Add("then");
            ignoring.Add("thence");
            ignoring.Add("there");
            ignoring.Add("thereafter");
            ignoring.Add("thereby");
            ignoring.Add("therefore");
            ignoring.Add("therein");
            ignoring.Add("thereupon");
            ignoring.Add("these");
            ignoring.Add("they");
            ignoring.Add("this");
            ignoring.Add("those");
            ignoring.Add("though");
            ignoring.Add("through");
            ignoring.Add("throughout");
            ignoring.Add("thru");
            ignoring.Add("thus");
            ignoring.Add("to");
            ignoring.Add("together");
            ignoring.Add("too");
            ignoring.Add("toward");
            ignoring.Add("towards");
            ignoring.Add("under");
            ignoring.Add("unless");
            ignoring.Add("until");
            ignoring.Add("up");
            ignoring.Add("upon");
            ignoring.Add("us");
            ignoring.Add("used");
            ignoring.Add("very");
            ignoring.Add("via");
            ignoring.Add("was");
            ignoring.Add("we");
            ignoring.Add("well");
            ignoring.Add("were");
            ignoring.Add("what");
            ignoring.Add("whatever");
            ignoring.Add("when");
            ignoring.Add("whence");
            ignoring.Add("whenever");
            ignoring.Add("where");
            ignoring.Add("whereafter");
            ignoring.Add("whereas");
            ignoring.Add("whereby");
            ignoring.Add("wherein");
            ignoring.Add("whereupon");
            ignoring.Add("wherever");
            ignoring.Add("whether");
            ignoring.Add("which");
            ignoring.Add("while");
            ignoring.Add("whither");
            ignoring.Add("who");
            ignoring.Add("whoever");
            ignoring.Add("whole");
            ignoring.Add("whom");
            ignoring.Add("whose");
            ignoring.Add("why");
            ignoring.Add("will");
            ignoring.Add("with");
            ignoring.Add("within");
            ignoring.Add("without");
            ignoring.Add("would");
            ignoring.Add("yes");
            ignoring.Add("yet");
            ignoring.Add("hey");
            ignoring.Add("you");
            ignoring.Add("your");
            ignoring.Add("yours");
            ignoring.Add("yourself");
            ignoring.Add("yourselves");

        }//




        public void answers(ArrayList add_answers)
        {//start of method




            add_answers.Add("greeting i'm doing well, thanks for asking! how are you doing today ?");
            add_answers.Add("greeting i'm great today, thanks for asking! how can i help you today ?");
            add_answers.Add("greeting doing good! hope you are also doing well today ?");
            add_answers.Add("greeting i'm feeling fantastic and ready to help you today!");
            add_answers.Add("greeting hello! i'm doing awesome, thanks for checking in!");

            add_answers.Add("purpose my purpose is to educate you on how to stay safe online and guide your cybersecurity questions.");
            add_answers.Add("purpose i help users understand online safety and digital protection.");
            add_answers.Add("purpose i assist with cybersecurity awareness and safety guidance.");
            add_answers.Add("purpose my role is to help users learn safe online practices.");
            add_answers.Add("purpose i am here to answer cybersecurity questions and provide guidance.");

            add_answers.Add("cybersecurity cybersecurity is about protecting systems and networks from digital threats.");
            add_answers.Add("cybersecurity it involves protecting devices and online accounts from attacks.");
            add_answers.Add("cybersecurity it focuses on securing digital information and systems.");
            add_answers.Add("cybersecurity cybersecurity helps prevent hackers from accessing sensitive information.");
            add_answers.Add("cybersecurity it protects computers, networks, and data from cyber attacks.");


            add_answers.Add("phishing phishing is a scam where attackers pretend to be trusted sources to steal information.");
            add_answers.Add("phishing it uses fake messages or websites to trick users into revealing sensitive data.");
            add_answers.Add("phishing attackers use deception to make users believe they are legitimate.");
            add_answers.Add("phishing phishing emails often pretend to come from banks or trusted companies.");
            add_answers.Add("phishing never click suspicious links or download unknown attachments.");


            add_answers.Add("firewall a firewall controls network traffic based on security rules.");
            add_answers.Add("firewall it helps block unwanted access to your device or network.");
            add_answers.Add("firewall it acts as a protective barrier between trusted and untrusted networks.");
            add_answers.Add("firewall firewalls help prevent hackers from accessing private networks.");
            add_answers.Add("firewall they monitor incoming and outgoing network traffic for threats.");

            add_answers.Add("password a password is used to secure access to your accounts or devices.");
            add_answers.Add("password it should be strong, long and not easy to guess.");
            add_answers.Add("password avoid using personal details when creating one.");
            add_answers.Add("password using unique passwords for every account improves security.");
            add_answers.Add("password a secure password usually contains symbols, numbers, and uppercase letters.");


            add_answers.Add("hacked account immediately secure your account and log out of all devices.");
            add_answers.Add("hacked account contact support if your account has been compromised.");
            add_answers.Add("hacked account enable extra security like two-factor authentication.");
            add_answers.Add("hacked account change your password immediately if you suspect hacking.");
            add_answers.Add("hacked account review your recent account activity for suspicious actions.");

            add_answers.Add("fraud contact your bank immediately if fraud is detected.");
            add_answers.Add("fraud report suspicious financial activity to the authorities.");
            add_answers.Add("fraud monitor your accounts for unusual activity.");
            add_answers.Add("fraud never share banking details with untrusted individuals.");
            add_answers.Add("fraud scammers often create urgency to trick victims into sending money.");



            add_answers.Add("malicious chatbot malicious bots often create urgency to trick users.");
            add_answers.Add("malicious chatbot fake chatbots may ask for sensitive information.");
            add_answers.Add("malicious chatbot be cautious if a bot pressures you for personal data.");
            add_answers.Add("malicious chatbot avoid bots that request passwords or banking information.");
            add_answers.Add("malicious chatbot trusted chatbots should never pressure users into quick decisions.");


            add_answers.Add("vpn a vpn helps protect your privacy on public wi-fi.");
            add_answers.Add("vpn it encrypts your internet traffic for safety.");
            add_answers.Add("vpn it improves security when using public networks.");
            add_answers.Add("vpn using a vpn can help keep your browsing activity private.");
            add_answers.Add("vpn vpns are useful when accessing the internet from public locations.");



            //sentiment detection

            add_answers.Add("frustrated i understand you're frustrated. let's work through the issue step by step.");
            add_answers.Add("frustrated it's okay to feel frustrated when things aren't working. i'm here to help.");
            add_answers.Add("frustrated take a breath, we'll fix this together.");


            add_answers.Add("confused that's okay, confusion is normal. i'll explain it clearly for you.");
            add_answers.Add("confused let me break it down step by step so it makes sense.");
            add_answers.Add("confused no worries, i'll help you understand it better.");


            add_answers.Add("worried it's okay to feel worried. i'm here to help you stay safe online.");
            add_answers.Add("worried don't panic, most cybersecurity issues can be fixed quickly.");
            add_answers.Add("worried i understand your concern. let's make sure your information is safe.");


            add_answers.Add("happy that's great to hear! i'm glad things are going well.");
            add_answers.Add("happy awesome! positivity is always good.");
            add_answers.Add("happy i'm happy for you! let me know if you need anything.");


            add_answers.Add("sad i'm sorry you're feeling this way. i'm here for you.");
            add_answers.Add("sad that sounds tough, take things one step at a time.");
            add_answers.Add("sad i hope things improve soon. you can talk to me anytime.");


            add_answers.Add("angry i understand you're angry. let's try solve the issue together.");
            add_answers.Add("angry it's okay to feel angry, but i'll help you fix the problem.");
            add_answers.Add("angry take your time, i'm here to help you sort it out.");





        }//end of method








    }
}
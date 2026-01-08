using Architypes.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Architypes.Infrastructure.Data;

public static class DbSeeder
{
    public static async Task SeedDataAsync(IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<ArchitypesDbContext>();

        // Check if data already exists
        if (await context.Archetypes.AnyAsync())
        {
            return; // Database already seeded
        }

        var archetypes = new List<Archetype>
        {
            new Archetype
            {
                Name = "King/Queen",
                MaleName = "The King",
                FemaleName = "The Queen",
                CoreDrive = "Order, responsibility, sovereignty",
                Strengths = "Leadership, decision-making, creating stability, taking ownership",
                Shadow = "Tyrant (controlling) or Abdicator (avoids responsibility)",
                InBusiness = "Executive leadership, department heads, founders",
                FreeTeaser = "You lead with sovereignty and responsibility. You create order where there is chaos and naturally take ownership of outcomes. People look to you to make final decisions and bring stability to complex situations.",
                DetailedCharacteristics = "The King/Queen archetype embodies the essence of mature, responsible leadership. You have a natural ability to see the bigger picture and make decisions that serve the collective good. You create structure and order not for control's sake, but to provide a stable foundation for others to thrive. Your presence commands respect, and you carry the weight of responsibility with grace. You understand that true leadership is about service to your domain—whether that's your team, family, or organization.",
                Blindspots = "You may struggle with delegation, believing that you must oversee everything personally. Your strong sense of responsibility can lead to burnout if you don't maintain boundaries. You might also become rigid in your thinking, preferring order to the point where you resist necessary change. In shadow, you can become tyrannical, using control to manage your fear of chaos, or conversely, you might abdicate responsibility when the burden feels too heavy.",
                InteractionPatterns = "Works well with Warriors (who execute your vision) and Fathers/Mothers (who nurture what you've built). May clash with Rebels (who challenge your authority) and Explorers (who resist structure). Needs Sages for wise counsel and Magicians to help transform systems."
            },
            new Archetype
            {
                Name = "Warrior/Huntress",
                MaleName = "The Warrior",
                FemaleName = "The Huntress",
                CoreDrive = "Discipline, courage, boundaries",
                Strengths = "Execution, perseverance, protecting what matters, focus",
                Shadow = "Sadist (aggression without purpose) or Masochist (self-destruction)",
                InBusiness = "Operations, sales, project management, crisis response",
                FreeTeaser = "You are disciplined, focused, and relentless in pursuit of your goals. You set firm boundaries and have the courage to face challenges head-on. Others rely on you to execute and protect what matters most.",
                DetailedCharacteristics = "The Warrior/Huntress is the archetype of focused action and protective strength. You embody discipline, not as rigid control, but as committed practice toward mastery. Your courage isn't the absence of fear—it's the willingness to move forward despite it. You understand that true strength comes from knowing what you're willing to fight for and protecting those boundaries fiercely. You bring clarity to action, cutting through confusion with decisive movement. Your persistence inspires others to push beyond their perceived limits.",
                Blindspots = "Your drive can become ruthless aggression when disconnected from purpose. You may push yourself or others too hard, seeing rest as weakness. Boundaries can become walls that isolate you. In shadow, you risk becoming sadistic (hurting others without purpose) or masochistic (destroying yourself in the name of discipline). You might also struggle to recognize when strategic retreat is wiser than continued advance.",
                InteractionPatterns = "Natural ally to Kings/Queens (executing their vision) and Heroes (sharing the drive for achievement). May conflict with Lovers (who prioritize feeling over action) and Jesters (whose playfulness feels frivolous). Benefits from Sages who provide strategic wisdom and Caregivers who remind you to rest."
            },
            new Archetype
            {
                Name = "Magician/Mystic",
                MaleName = "The Magician",
                FemaleName = "The Mystic",
                CoreDrive = "Transformation, insight, mastery",
                Strengths = "Pattern recognition, problem-solving, connecting disparate ideas",
                Shadow = "Manipulator (uses knowledge to control) or Detached Dreamer",
                InBusiness = "Systems architecture, integration, automation, consulting",
                FreeTeaser = "You see patterns and connections others miss. You transform complex problems into elegant solutions and are the one people turn to when something 'impossible' needs solving.",
                DetailedCharacteristics = "The Magician/Mystic possesses the rare ability to perceive the hidden patterns underlying reality. You see connections between disparate elements that others miss, enabling you to transform problems into opportunities through insight rather than force. Your mastery comes from deep understanding of how systems work—whether technical, organizational, or human. You bring the gift of possibility, showing others that what seemed impossible is merely misunderstood. Your power lies in knowledge and the ability to apply it creatively.",
                Blindspots = "Your understanding can lead to manipulation when you use knowledge to control rather than liberate. You may become so focused on the theoretical that you lose touch with practical reality, becoming the detached dreamer lost in abstraction. Your ability to see multiple perspectives can lead to paralysis or to using complexity to avoid commitment. You might also become arrogant, believing your insights make you superior.",
                InteractionPatterns = "Partners well with Sages (sharing love of knowledge) and Creators (who bring your insights into form). May frustrate Warriors (who want action over analysis) and Kings/Queens (who need clear recommendations). Complements Explorers by giving their discoveries structure."
            },
            new Archetype
            {
                Name = "Lover",
                MaleName = "The Lover",
                FemaleName = "The Lover",
                CoreDrive = "Connection, passion, aliveness",
                Strengths = "Deep engagement, appreciation of beauty, emotional intelligence",
                Shadow = "Addicted Lover (obsession) or Impotent Lover (numbness)",
                InBusiness = "Customer experience, brand, design, culture",
                FreeTeaser = "You engage deeply with life, beauty, and meaning. Passion flows through everything you do, and you form strong emotional connections with people and work. For you, a life without depth and feeling isn't worth living.",
                DetailedCharacteristics = "The Lover embodies passion, connection, and full-bodied engagement with life. You experience the world through feeling and sensation, bringing intensity and depth to everything you touch. Beauty matters to you—not as superficial decoration, but as the aesthetic expression of meaning. You form deep bonds and commit fully when you care about something or someone. Your gift is the ability to be fully present, to savor life rather than merely accomplish tasks. You remind others why they're alive.",
                Blindspots = "Your passion can become obsession, losing yourself in what you love to the point of losing yourself. You may struggle with boundaries, getting too enmeshed in relationships or projects. In shadow, you can become addicted to intensity, chasing the high of connection without substance, or conversely, you may shut down entirely, becoming numb to protect yourself from feeling too much. Your need for meaning can make practical necessities feel empty.",
                InteractionPatterns = "Natural affinity with Caregivers (who share your connection focus) and Creators (who channel passion into form). May clash with Warriors (whose discipline feels cold) and Sages (whose detachment feels sterile). Benefits from Jesters who share your appreciation for life's pleasure."
            },
            new Archetype
            {
                Name = "Sage",
                MaleName = "The Sage",
                FemaleName = "The Sage",
                CoreDrive = "Truth, understanding, wisdom",
                Strengths = "Analysis, reflection, objectivity, teaching",
                Shadow = "Disconnected Sage (ivory tower) or Dogmatist (closed-minded)",
                InBusiness = "Research, strategy, compliance, training, advisory",
                FreeTeaser = "You seek deep understanding above all else. Truth matters more to you than comfort, and you're constantly seeking knowledge and wisdom. You'd rather fully understand something than act quickly on partial information.",
                DetailedCharacteristics = "The Sage is driven by an insatiable thirst for truth and understanding. You approach life with curiosity and discernment, seeking wisdom through study, reflection, and careful observation. Your gift is the ability to cut through illusion and see what is actually true, not merely what is believed or comfortable. You value objectivity and can step back from emotional entanglement to gain perspective. Your knowledge serves others through teaching and counsel, helping them see clearly what they couldn't perceive before.",
                Blindspots = "Your pursuit of truth can lead you into ivory tower detachment, accumulating knowledge without applying it. You may use the need for 'more information' to avoid action or commitment. In shadow, you can become dogmatic, believing your understanding is the only truth, or cynical, using your insights to tear down without building. Your detachment can make you seem cold or uncaring, and you might undervalue emotional or experiential knowledge.",
                InteractionPatterns = "Strong connection with Magicians (sharing love of understanding) and Fathers/Mothers (who teach wisdom). May frustrate Lovers (who prioritize feeling) and Explorers (who value experience over study). Serves Kings/Queens well as trusted advisors."
            },
            new Archetype
            {
                Name = "Explorer/Wild Woman",
                MaleName = "The Explorer",
                FemaleName = "The Wild Woman",
                CoreDrive = "Freedom, discovery, autonomy",
                Strengths = "Innovation, adaptability, pioneering, independence",
                Shadow = "Wanderer (aimless) or Escapist (avoids commitment)",
                InBusiness = "R&D, market expansion, entrepreneurship, field roles",
                FreeTeaser = "You need freedom and novelty like others need air. Routine suffocates you, and you're drawn to the unknown and unexplored. You resist being tied down to any single path, always seeking the next horizon.",
                DetailedCharacteristics = "The Explorer/Wild Woman is the archetype of freedom, discovery, and the untamed spirit. You resist domestication in all its forms, needing autonomy to feel alive. Your gift is pioneering—you go where others haven't, bringing back discoveries that expand what's possible for everyone. You adapt quickly because you're not attached to any particular way of being. Your independence isn't selfish; it's necessary for you to bring back the innovations and experiences that routine-bound society desperately needs.",
                Blindspots = "Your love of freedom can become aimless wandering, moving for the sake of movement without clear purpose. You may avoid commitment entirely, mistaking attachment for entrapment. In shadow, you can become an escapist, running from difficulty rather than toward discovery. Your independence might isolate you, and your resistance to structure can prevent you from building anything lasting. You may also romanticize the new while dismissing what's already been built.",
                InteractionPatterns = "Works well with Creators (bringing fresh perspectives) and Rebels (sharing distrust of constraint). May clash with Kings/Queens (who provide structure) and Warriors (who value discipline). Complements Magicians by testing theories in the real world."
            },
            new Archetype
            {
                Name = "Creator/Creatrix",
                MaleName = "The Creator",
                FemaleName = "The Creatrix",
                CoreDrive = "Innovation, expression, bringing new things into being",
                Strengths = "Originality, vision, craftsmanship, artistic sensibility",
                Shadow = "Perfectionist (never finishes) or Tortured Artist (suffers for creation)",
                InBusiness = "Product development, marketing, design, content, entrepreneurship",
                FreeTeaser = "You're driven to build original things—ideas, systems, products that didn't exist before. You feel most alive when creating something new and see possibilities where others see only blank space.",
                DetailedCharacteristics = "The Creator/Creatrix brings new realities into being through imagination and craftsmanship. You see potential where others see nothing, and you possess both the vision to conceive new possibilities and the skill to manifest them. Your creations carry your unique signature—whether you're building products, organizations, or works of art. You understand that creation is sacred work, the act of bringing something from the invisible realm of imagination into tangible form. Your gift enriches the world with beauty, utility, and meaning.",
                Blindspots = "Your vision can become perfectionism, where nothing is ever good enough to share with the world. You may suffer for your art unnecessarily, believing pain is required for creation. In shadow, you can become the tortured artist, so identified with your wounds that you fear healing would end your creativity. You might also create compulsively, using the act of making to avoid being present. Your high standards can make you harshly critical of others' work.",
                InteractionPatterns = "Natural partners with Lovers (sharing aesthetic sensibility) and Magicians (who provide creative insight). May frustrate Warriors (who want finished products) and Sages (who want practical utility). Benefits from Kings/Queens who provide structure for your visions."
            },
            new Archetype
            {
                Name = "Hero",
                MaleName = "The Hero",
                FemaleName = "The Hero",
                CoreDrive = "Mastery, achievement, proving worth",
                Strengths = "Courage, competence, resilience, inspiring others",
                Shadow = "Bully (dominates) or Coward (avoids challenge)",
                InBusiness = "Sales, competitive roles, turnaround situations, athletics",
                FreeTeaser = "You prove yourself through achievement and overcoming challenges. Competition energizes you, and you need to know you've earned your place through demonstrated competence and courage.",
                DetailedCharacteristics = "The Hero is driven by the need to prove their worth through achievement and mastery. You face challenges that others avoid, not from recklessness but from the conviction that you can overcome them. Your courage inspires others to find their own strength. You understand that true competence comes from testing yourself against real obstacles, not from theoretical knowledge. Your resilience shows others that setbacks are temporary, and your victories demonstrate what's possible when you refuse to quit.",
                Blindspots = "Your drive for achievement can become an endless need to prove yourself, never satisfied with what you've accomplished. You may become a bully, dominating others to feel superior, or conversely, a coward who avoids challenges that might reveal limitations. In shadow, you can confuse your worth with your accomplishments, losing your sense of self when you're not actively winning. You might also create unnecessary conflicts to have dragons to slay, or burn yourself out pursuing achievement for achievement's sake.",
                InteractionPatterns = "Strong alliance with Warriors (sharing drive for achievement) and Kings/Queens (who reward competence). May compete unnecessarily with other Heroes or dismiss Caregivers as weak. Benefits from Sages who provide perspective on what's worth achieving."
            },
            new Archetype
            {
                Name = "Rebel/Outlaw",
                MaleName = "The Rebel",
                FemaleName = "The Outlaw",
                CoreDrive = "Liberation, revolution, breaking false structures",
                Strengths = "Challenging status quo, courage, authenticity, change agency",
                Shadow = "Criminal (destruction without purpose) or Self-Saboteur",
                InBusiness = "Innovation, disruption, change management, turnaround",
                FreeTeaser = "You question authority and resist arbitrary structures. You'd rather break rules that don't make sense than blindly follow them, and you see through systems that others accept without question.",
                DetailedCharacteristics = "The Rebel/Outlaw possesses the rare courage to challenge systems that everyone else accepts as unchangeable. You see through social conditioning and question authority that hasn't earned respect through wisdom or justice. Your gift is liberation—you break false structures so that something more authentic can emerge. You're willing to stand alone against the crowd when you know something is wrong, and your authenticity inspires others to be true to themselves rather than conforming to expectations.",
                Blindspots = "Your rebellion can become destruction for its own sake, tearing down without building anything better. You may become the criminal, breaking rules without higher purpose, or the self-saboteur, destroying your own success because it feels too conventional. In shadow, you might rebel reflexively against any authority, even when it's wise and just. Your distrust of systems can leave you unable to work within any structure, and your outsider status might become your identity, preventing you from creating the change you wish to see.",
                InteractionPatterns = "Allies with Explorers (sharing distrust of constraint) and Creators (who build alternatives to broken systems). May clash with Kings/Queens (who represent authority) and Warriors (who enforce rules). Complements Magicians who can reimagine broken systems."
            },
            new Archetype
            {
                Name = "Jester/Fool",
                MaleName = "The Jester",
                FemaleName = "The Fool",
                CoreDrive = "Joy, presence, truth through humor",
                Strengths = "Lightness, perspective, cutting through pretension, living in the moment",
                Shadow = "Cruel Joker (humor as weapon) or Self-Deprecator",
                InBusiness = "Creative, culture building, facilitation, sales, entertainment",
                FreeTeaser = "You use humor to reveal truth and cut through tension. You don't take yourself or life too seriously, bringing lightness to heavy situations and helping others find perspective through laughter.",
                DetailedCharacteristics = "The Jester/Fool brings the sacred gift of perspective and presence. Your humor isn't mere entertainment—it's a vehicle for truth-telling, cutting through pretension and self-importance to reveal what's real. You understand that laughter creates connection and that playfulness is a form of wisdom. By not taking yourself too seriously, you're free to be authentic and to help others do the same. You remind people that joy and meaning coexist, that life can be both profound and playful.",
                Blindspots = "Your humor can become a weapon, using jokes to wound rather than illuminate. You may hide behind levity, using laughter to avoid genuine intimacy or serious responsibility. In shadow, you can become the cruel joker, or conversely, the self-deprecator who turns all humor inward in self-destructive ways. Your presence-focus might make planning for the future feel unnecessary, and your refusal to take things seriously might prevent you from building anything lasting.",
                InteractionPatterns = "Natural connection with Lovers (sharing appreciation for life's pleasure) and Explorers (who share playfulness). May frustrate Warriors (who see frivolity) and Sages (who want depth). Balances well with Kings/Queens who need reminder not to take themselves too seriously."
            },
            new Archetype
            {
                Name = "Caregiver/Healer",
                MaleName = "The Caregiver",
                FemaleName = "The Healer",
                CoreDrive = "Service, compassion, nurturing",
                Strengths = "Empathy, generosity, creating safety, supporting growth",
                Shadow = "Martyr (gives until depleted) or Enabler (helps harmfully)",
                InBusiness = "HR, customer success, healthcare, support, coaching",
                FreeTeaser = "You prioritize others' wellbeing, sometimes at your own expense. You're drawn to help, fix, and restore, feeling most purposeful when serving and supporting others' growth and healing.",
                DetailedCharacteristics = "The Caregiver/Healer embodies compassion and service. You possess deep empathy, feeling others' pain and joy as if they were your own. Your gift is creating safety—physical, emotional, and spiritual—where others can heal and grow. You're generous with your time, energy, and resources, finding meaning through supporting others. You understand that true care requires seeing people fully, not fixing them but witnessing and supporting their own journey toward wholeness.",
                Blindspots = "Your care can become martyrdom, giving so much that you deplete yourself and then resenting those you serve. You may enable others' dysfunction by helping in ways that prevent their growth. In shadow, you might need others to be broken so you can feel needed, or you might care so much for others that you neglect yourself entirely. Your empathy can become emotional enmeshment, taking on others' feelings without maintaining your own center.",
                InteractionPatterns = "Partners well with Lovers (sharing emotional depth) and Fathers/Mothers (both focused on nurturing). May enable Heroes to avoid their shadows or exhaust themselves caring for Rebels. Benefits from Warriors who protect your boundaries and Sages who provide perspective."
            },
            new Archetype
            {
                Name = "Father/Mother",
                MaleName = "The Father",
                FemaleName = "The Mother",
                CoreDrive = "Protection, guidance, provision",
                Strengths = "Nurturing growth, establishing structure, wisdom, patience",
                Shadow = "Devouring Parent (overcontrol) or Abandoning Parent",
                InBusiness = "Mentorship, team leadership, coaching, education",
                FreeTeaser = "You invest heavily in developing and guiding others. You create structure that helps people grow and feel responsible for preparing them for their future, combining nurture with wisdom.",
                DetailedCharacteristics = "The Father/Mother archetype embodies mature, generative care focused on preparing others for their own journey. Unlike the Caregiver who tends wounds, you build capacity. You provide structure, wisdom, and resources that enable others' development. Your love is expressed through investment in others' potential, combining nurture with appropriate challenge. You understand that true parenting—of children, teams, or protégés—means preparing them to leave you, to become autonomous and capable. Your legacy lives in those you've developed.",
                Blindspots = "Your guidance can become control, the devouring parent who prevents growth by never letting go. You may live vicariously through others' achievements, or conversely, abandon them before they're ready in the name of 'letting them learn.' In shadow, you might need others to remain dependent to feel valuable, or you might be so focused on their future that you're not present to who they are now. Your wisdom can become dogma if you're not willing to learn from those you teach.",
                InteractionPatterns = "Works well with Kings/Queens (both focused on long-term stability) and Sages (sharing wisdom). May overprotect Heroes from necessary challenges or clash with Rebels who resist guidance. Complements Caregivers by focusing on growth rather than healing."
            }
        };

        await context.Archetypes.AddRangeAsync(archetypes);
        await context.SaveChangesAsync();

        // Now create questions for each archetype
        var questions = new List<Question>();
        var archetypesList = await context.Archetypes.ToListAsync();

        int displayOrder = 1;

        // King/Queen questions
        var kingQueen = archetypesList.First(a => a.Name == "King/Queen");
        questions.Add(new Question { Text = "I naturally take charge and feel responsible for outcomes in groups", ArchetypeId = kingQueen.Id, DisplayOrder = displayOrder++ });
        questions.Add(new Question { Text = "People look to me to make final decisions", ArchetypeId = kingQueen.Id, DisplayOrder = displayOrder++ });
        questions.Add(new Question { Text = "I feel accountable for the success of my team/family/domain", ArchetypeId = kingQueen.Id, DisplayOrder = displayOrder++ });

        // Warrior/Huntress questions
        var warrior = archetypesList.First(a => a.Name == "Warrior/Huntress");
        questions.Add(new Question { Text = "I push through resistance and finish what I start", ArchetypeId = warrior.Id, DisplayOrder = displayOrder++ });
        questions.Add(new Question { Text = "I set firm boundaries and enforce them", ArchetypeId = warrior.Id, DisplayOrder = displayOrder++ });
        questions.Add(new Question { Text = "I'm energised by challenges that test my limits", ArchetypeId = warrior.Id, DisplayOrder = displayOrder++ });

        // Magician/Mystic questions
        var magician = archetypesList.First(a => a.Name == "Magician/Mystic");
        questions.Add(new Question { Text = "I see patterns and connections others miss", ArchetypeId = magician.Id, DisplayOrder = displayOrder++ });
        questions.Add(new Question { Text = "I transform complex problems into elegant solutions", ArchetypeId = magician.Id, DisplayOrder = displayOrder++ });
        questions.Add(new Question { Text = "I'm the one people call when something 'impossible' needs solving", ArchetypeId = magician.Id, DisplayOrder = displayOrder++ });

        // Lover questions
        var lover = archetypesList.First(a => a.Name == "Lover");
        questions.Add(new Question { Text = "I engage deeply with experiences—beauty and meaning matter intensely", ArchetypeId = lover.Id, DisplayOrder = displayOrder++ });
        questions.Add(new Question { Text = "I form strong emotional connections with people and work", ArchetypeId = lover.Id, DisplayOrder = displayOrder++ });
        questions.Add(new Question { Text = "A life without passion isn't worth living", ArchetypeId = lover.Id, DisplayOrder = displayOrder++ });

        // Sage questions
        var sage = archetypesList.First(a => a.Name == "Sage");
        questions.Add(new Question { Text = "I'd rather understand something fully than act quickly", ArchetypeId = sage.Id, DisplayOrder = displayOrder++ });
        questions.Add(new Question { Text = "I value truth over comfort", ArchetypeId = sage.Id, DisplayOrder = displayOrder++ });
        questions.Add(new Question { Text = "I'm constantly seeking deeper knowledge", ArchetypeId = sage.Id, DisplayOrder = displayOrder++ });

        // Explorer/Wild Woman questions
        var explorer = archetypesList.First(a => a.Name == "Explorer/Wild Woman");
        questions.Add(new Question { Text = "Routine suffocates me; I need novelty and autonomy", ArchetypeId = explorer.Id, DisplayOrder = displayOrder++ });
        questions.Add(new Question { Text = "I'm drawn to the unknown and unexplored", ArchetypeId = explorer.Id, DisplayOrder = displayOrder++ });
        questions.Add(new Question { Text = "I resist being tied down to one path", ArchetypeId = explorer.Id, DisplayOrder = displayOrder++ });

        // Creator/Creatrix questions
        var creator = archetypesList.First(a => a.Name == "Creator/Creatrix");
        questions.Add(new Question { Text = "I'm driven to build original things—ideas, systems, products", ArchetypeId = creator.Id, DisplayOrder = displayOrder++ });
        questions.Add(new Question { Text = "I feel most alive when creating something new", ArchetypeId = creator.Id, DisplayOrder = displayOrder++ });
        questions.Add(new Question { Text = "I see possibilities where others see blank space", ArchetypeId = creator.Id, DisplayOrder = displayOrder++ });

        // Hero questions
        var hero = archetypesList.First(a => a.Name == "Hero");
        questions.Add(new Question { Text = "I prove myself through achievement and overcoming challenges", ArchetypeId = hero.Id, DisplayOrder = displayOrder++ });
        questions.Add(new Question { Text = "I'm energised by competition and winning", ArchetypeId = hero.Id, DisplayOrder = displayOrder++ });
        questions.Add(new Question { Text = "I need to know I've earned my place", ArchetypeId = hero.Id, DisplayOrder = displayOrder++ });

        // Rebel/Outlaw questions
        var rebel = archetypesList.First(a => a.Name == "Rebel/Outlaw");
        questions.Add(new Question { Text = "I question authority and resist structures that feel arbitrary", ArchetypeId = rebel.Id, DisplayOrder = displayOrder++ });
        questions.Add(new Question { Text = "I'd rather break the rules than follow ones that don't make sense", ArchetypeId = rebel.Id, DisplayOrder = displayOrder++ });
        questions.Add(new Question { Text = "I see through systems others accept blindly", ArchetypeId = rebel.Id, DisplayOrder = displayOrder++ });

        // Jester/Fool questions
        var jester = archetypesList.First(a => a.Name == "Jester/Fool");
        questions.Add(new Question { Text = "I use humor to cut through tension and reveal truth", ArchetypeId = jester.Id, DisplayOrder = displayOrder++ });
        questions.Add(new Question { Text = "I don't take myself or life too seriously", ArchetypeId = jester.Id, DisplayOrder = displayOrder++ });
        questions.Add(new Question { Text = "I bring lightness to heavy situations", ArchetypeId = jester.Id, DisplayOrder = displayOrder++ });

        // Caregiver/Healer questions
        var caregiver = archetypesList.First(a => a.Name == "Caregiver/Healer");
        questions.Add(new Question { Text = "I prioritize others' wellbeing, sometimes at my own expense", ArchetypeId = caregiver.Id, DisplayOrder = displayOrder++ });
        questions.Add(new Question { Text = "I'm drawn to help, fix, and restore", ArchetypeId = caregiver.Id, DisplayOrder = displayOrder++ });
        questions.Add(new Question { Text = "I feel most purposeful when serving others", ArchetypeId = caregiver.Id, DisplayOrder = displayOrder++ });

        // Father/Mother questions
        var parent = archetypesList.First(a => a.Name == "Father/Mother");
        questions.Add(new Question { Text = "I invest heavily in developing and guiding others", ArchetypeId = parent.Id, DisplayOrder = displayOrder++ });
        questions.Add(new Question { Text = "I create structure that helps people grow", ArchetypeId = parent.Id, DisplayOrder = displayOrder++ });
        questions.Add(new Question { Text = "I feel responsible for preparing others for their future", ArchetypeId = parent.Id, DisplayOrder = displayOrder++ });

        await context.Questions.AddRangeAsync(questions);
        await context.SaveChangesAsync();
    }
}

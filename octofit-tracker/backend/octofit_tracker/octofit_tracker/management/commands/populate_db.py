from django.core.management.base import BaseCommand
from django.contrib.auth.hashers import make_password
from octofit_tracker.models import User, Team, Activity, LeaderboardEntry, Workout


class Command(BaseCommand):
    help = 'Populate the database with sample data for OctoFit Tracker'

    def handle(self, *args, **kwargs):
        self.stdout.write('Clearing existing data...')
        LeaderboardEntry.objects.all().delete()
        User.objects.all().delete()
        Team.objects.all().delete()
        Activity.objects.all().delete()
        Workout.objects.all().delete()

        self.stdout.write('Creating users...')
        users_data = [
            {'username': 'spider_man', 'email': 'peter.parker@mergington.edu', 'password': make_password('spidey123')},
            {'username': 'iron_man', 'email': 'tony.stark@mergington.edu', 'password': make_password('ironman123')},
            {'username': 'black_widow', 'email': 'natasha.romanoff@mergington.edu', 'password': make_password('widow123')},
            {'username': 'thor', 'email': 'thor.odinson@mergington.edu', 'password': make_password('mjolnir123')},
            {'username': 'captain_america', 'email': 'steve.rogers@mergington.edu', 'password': make_password('shield123')},
        ]
        users = []
        for data in users_data:
            user = User.objects.create(**data)
            users.append(user)
            self.stdout.write(f'  Created user: {user.username}')

        self.stdout.write('Creating teams...')
        team_marvel = Team.objects.create(
            name='Team Marvel',
            members=['spider_man', 'iron_man', 'black_widow']
        )
        team_dc = Team.objects.create(
            name='Team DC',
            members=['thor', 'captain_america']
        )
        self.stdout.write(f'  Created team: {team_marvel.name}')
        self.stdout.write(f'  Created team: {team_dc.name}')

        self.stdout.write('Creating activities...')
        activities_data = [
            {
                'name': 'Morning Run',
                'description': 'Start your day with a refreshing run',
                'schedule': 'Mon/Wed/Fri at 6am',
                'max_participants': 30,
            },
            {
                'name': 'Yoga Class',
                'description': 'Relax and strengthen with yoga',
                'schedule': 'Tuesdays at 5pm',
                'max_participants': 20,
            },
            {
                'name': 'Swimming',
                'description': 'Swim laps in the pool',
                'schedule': 'Thursdays at 4pm',
                'max_participants': 15,
            },
            {
                'name': 'Manga Maniacs',
                'description': 'Explore the fantastic stories of the most interesting characters from Japanese Manga (graphic novels).',
                'schedule': 'Tuesdays at 7pm',
                'max_participants': 15,
            },
        ]
        for data in activities_data:
            activity = Activity.objects.create(**data)
            self.stdout.write(f'  Created activity: {activity.name}')

        self.stdout.write('Creating workouts...')
        workouts_data = [
            {
                'name': 'Strength Training',
                'description': 'Build muscle with weightlifting exercises including squats, deadlifts, and bench press.',
                'duration': 60,
            },
            {
                'name': 'Cardio Blast',
                'description': 'High-intensity cardiovascular workout to improve endurance and burn calories.',
                'duration': 45,
            },
            {
                'name': 'Flexibility Flow',
                'description': 'Improve flexibility and range of motion with stretching and mobility exercises.',
                'duration': 30,
            },
            {
                'name': 'HIIT Circuit',
                'description': 'High-Intensity Interval Training combining strength and cardio for maximum results.',
                'duration': 40,
            },
        ]
        for data in workouts_data:
            workout = Workout.objects.create(**data)
            self.stdout.write(f'  Created workout: {workout.name}')

        self.stdout.write('Creating leaderboard entries...')
        scores = [950, 870, 820, 750, 680]
        for user, score in zip(users, scores):
            entry = LeaderboardEntry.objects.create(user=user, score=score)
            self.stdout.write(f'  Created leaderboard entry: {user.username} - {score}')

        self.stdout.write(self.style.SUCCESS('Database populated successfully!'))

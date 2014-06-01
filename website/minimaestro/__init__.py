import json
import os
from flask import Flask, render_template

app = Flask(__name__)

THEME_FILE = os.path.join(os.path.dirname(__file__), 'theme_settings.json')


def theme_settings():
    with open(THEME_FILE) as f:
        return json.load(f)


@app.route('/')
def home():
    return render_template('strapped.html', **theme_settings())

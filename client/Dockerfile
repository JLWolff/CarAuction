# ---------- build ----------
FROM node:19-alpine AS build
WORKDIR /app

# disable telemetry
ENV NEXT_TELEMETRY_DISABLED=1

# install dependencies
COPY package*.json ./
RUN npm ci

# build Next.js
COPY . .
RUN npm run build

# ---------- run ----------
FROM node:19-alpine AS runner
WORKDIR /app
ENV NODE_ENV=production
ENV PORT=3000
EXPOSE 3000

# add compat libs (needed by sharp/next/image on Alpine)
RUN apk add --no-cache libc6-compat

# run as non-root
RUN addgroup -g 1001 -S nodejs && adduser -S nextjs -u 1001
USER nextjs

# copy only the build output
COPY --from=build --chown=nextjs:nodejs /app/.next/standalone ./
COPY --from=build --chown=nextjs:nodejs /app/.next/static ./.next/static
COPY --from=build --chown=nextjs:nodejs /app/public ./public

CMD ["node", "server.js"]
